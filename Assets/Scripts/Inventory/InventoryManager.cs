using System;
using System.Collections.Generic;
using System.Linq;
using AlexMod.Inventory.Items;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using Utility;

namespace AlexMod.Inventory {
    public class InventoryManager : NetworkBehaviour {

        public event Action<ItemType, Item> OnItemRemoved;
        public event Action<ItemType, Item> OnItemAdded;
        public event Action<Item> ItemUsed;
        public event Action<ItemType> OnNewActiveItem;

        public Item ActiveItem => _currentItems.TryGetValue(_activeItemType, out Item asset) ? asset : null;
        
        [SyncVar(OnChange = nameof(ActiveItemChanged))]
        private ItemType _activeItemType;

        [SyncObject]
        private readonly SyncDictionary<ItemType, Item> _currentItems = new SyncDictionary<ItemType, Item>();

        [SerializeField] private Transform _itemParent;
        
        private void Awake() => _currentItems.OnChange += HandleItemChange;

        [Server]
        public void InsertItem(Item item) {
            RemoveItem(item.Type);
            _currentItems.Add(item.Type, item);
        }
        
        public void RemoveActiveItem() => RemoveItem(_activeItemType);
        
        public void RemoveItem(ItemType type) {
            if(!IsServer && !IsOwner) return;
            if (!_currentItems.ContainsKey(type)) return;
            _currentItems.Remove(type);
        }
        
        private void HandleItemChange(SyncDictionaryOperation op, ItemType key, Item value, bool asServer) {
            switch (op) {
                
                // parent the item under our transform
                case SyncDictionaryOperation.Add:
                    value.HandlePickUp(_itemParent);
                    if(key == _activeItemType) value.Select();
                    OnItemAdded.TryInvoke(key, value);
                    break;
                
                // drop the item
                case SyncDictionaryOperation.Remove:
                    value.HandleDrop(_itemParent.forward);
                    OnItemRemoved.TryInvoke(key, value);
                    break;
            }
        }

        [ServerRpc(RequireOwnership = true)]
        public void UseActiveItem() {
            if(ActiveItem == null) return;
            ActiveItem.Use(gameObject, _itemParent.forward);
            ItemUsed.TryInvoke(ActiveItem);
            InformClientsItemUse();
        }

        [ObserversRpc(IncludeOwner = true)]
        private void InformClientsItemUse() {
            if(ActiveItem == null) return;
            ItemUsed.TryInvoke(ActiveItem);
        }
        
        public void ActivateItem(ItemType type) {
            if(!IsServer && !IsOwner) return;
            _activeItemType = type;
        }

        // invoked by the syncvar for client and server
        private void ActiveItemChanged(ItemType prev, ItemType next, bool asServer) {
            foreach (KeyValuePair<ItemType, Item> itemPair in _currentItems) {
                if(itemPair.Key == next) itemPair.Value.Select();
                else itemPair.Value.Unselect();
            }
            OnNewActiveItem.TryInvoke(next);
        }

        public void ActivateNextItem() {
            if(!IsServer && !IsOwner) return;
            List<ItemType> allTypes = Enum.GetValues(typeof(ItemType)).OfType<ItemType>().ToList();
            // find the first itemType after our current one
            ItemType newType = allTypes
                .SkipWhile(x => x != _activeItemType).Skip(1)
                .DefaultIfEmpty( allTypes[0] ).FirstOrDefault();
            ActivateItem(newType);
        }
        
        public void ActivatePreviousItem() {
            if(!IsServer && !IsOwner) return;
            List<ItemType> allTypes = Enum.GetValues(typeof(ItemType)).OfType<ItemType>().ToList();
            // find the first itemType before our current one
            ItemType newType = allTypes
                .TakeWhile(x => x != _activeItemType)
                .DefaultIfEmpty( allTypes[allTypes.Count - 1]).LastOrDefault();
            ActivateItem(newType);
        }
    }
}