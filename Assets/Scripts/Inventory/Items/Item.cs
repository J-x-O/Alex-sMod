using System;
using FishNet.Object;
using UnityEngine;
using Utility;

namespace AlexMod.Inventory.Items {
    
    public enum ItemType { Small, Huge, Secondary }
    
    public abstract class Item : Interactable {

        public event Action OnItemUsed;
        public event Action OnItemPickUp;
        public event Action OnItemDrop;
        public event Action OnItemSelected;
        public event Action OnItemUnselected;
        
        public ItemType Type => _type;
        [SerializeField] private ItemType _type;

        [Header("Drop Logic")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _dropStrength;

        private bool _selected;
        
        public override void Interact(GameObject user) {
            if(!user.TryGetComponent(out InventoryManager manager)) return;
            manager.InsertItem(this);
        }

        [Server]
        public void Use(GameObject user, Vector3 direction) {
            UseInternal(user, direction);
            OnItemUsed.TryInvoke();
            InformClients();
        }
        
        [ObserversRpc(IncludeOwner = true)]
        public void InformClients() => OnItemUsed.TryInvoke();
        
        /// <summary> Use Logic will only be executed of the server </summary>
        protected abstract void UseInternal(GameObject user, Vector3 direction);

        public void HandlePickUp(Transform itemParent) {
            Transform ownTransform = transform;
            ownTransform.SetParent(itemParent);
            ownTransform.localPosition = Vector3.zero;
            ownTransform.localRotation = Quaternion.identity;
            _rigidbody.isKinematic = true;
            _selected = false;
            OnItemPickUp?.Invoke();
        }
        
        public void HandleDrop(Vector3 forward) {
            transform.parent = null;
            _rigidbody.isKinematic = false;
            Vector3 impact = forward.normalized * _dropStrength;
            _rigidbody.AddForce(impact);
            OnItemDrop?.Invoke();
        }

        public void Select() {
            if(_selected) return;
            _selected = true;
            OnItemSelected.TryInvoke();
        }
        
        public void Unselect() {
            if(!_selected) return;
            _selected = false;
            OnItemUnselected.TryInvoke();
        }
    }
}