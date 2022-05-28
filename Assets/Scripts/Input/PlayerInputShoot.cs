using System;
using AlexMod.Inventory;
using AlexMod.Inventory.Items;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AlexMod.Input {
    public class PlayerInputShoot : NetworkBehaviour {

        [SerializeField] private InteractionManager _interaction;
        [SerializeField] private InventoryManager _inventory;
        [SerializeField] private Item _debugItemPrefab;

        public override void OnStartClient() {
            base.OnStartClient();
            if (!IsOwner) return;
            PlayerInputManager.Asset.Player.Fire.performed += HandleShoot;
            PlayerInputManager.Asset.Player.Interact.performed += HandleInteract;
            PlayerInputManager.Asset.Player.Throw.performed += HandleThrow;
            PlayerInputManager.Asset.Player.SpawnDebugWeapon.performed += SpawnDebugWeapon;
        }

        public override void OnStopClient() {
            base.OnStopClient();
            if (!IsOwner) return;
            PlayerInputManager.Asset.Player.Fire.performed -= HandleShoot;
            PlayerInputManager.Asset.Player.Interact.performed -= HandleInteract;
            PlayerInputManager.Asset.Player.Throw.performed -= HandleThrow;
            PlayerInputManager.Asset.Player.SpawnDebugWeapon.performed -= SpawnDebugWeapon;
        }
        
        
        private void HandleShoot(InputAction.CallbackContext obj) => _inventory.UseActiveItem();

        private void HandleInteract(InputAction.CallbackContext obj) => _interaction.Interact();

        private void HandleThrow(InputAction.CallbackContext obj) => _inventory.RemoveActiveItem();

        private void SpawnDebugWeapon(InputAction.CallbackContext obj) {
            Item debugItem = Instantiate(_debugItemPrefab, transform.position, transform.rotation);
            ServerManager.Spawn(debugItem.gameObject, null);
        }
    }
}