using System;
using AlexMod.Inventory.Items;
using AlexMod.Utility;
using FishNet.Managing.Timing;
using FishNet.Object;
using UnityEngine;

namespace AlexMod.Inventory {
    public class InteractionManager : NetworkBehaviour {

        public float Range => _range;
        [SerializeField] private float _range;
        [SerializeField, Range(0,1)] private float _angle;
        [SerializeField] private Transform _pov;
        
        /// <summary> Only tracked on the Client! </summary>
        public Interactable CurrentInteractable { get; private set; }
        
        /// <summary> Only on the client, uploads the interactable to the server to validate it </summary>
        [Client(RequireOwnership = true)]
        public void Interact() => Interact(CurrentInteractable);

        [ServerRpc(RequireOwnership = true)]
        private void Interact(Interactable interactable) {
            if(interactable == null) return;
            
            float distance = Vector3.Distance(transform.position, interactable.transform.position);
            if(distance > Range) return;
            
            interactable.Interact(gameObject);
        }

        private void Update() {
            float closestAngle = 1;
            foreach (Interactable interactable in Interactable.Interactables) {
                Vector3 interactPos = interactable.transform.position;
                Vector3 ownPos = transform.position;
                
                // interactable is in range
                float distance = Vector3.Distance(interactPos, ownPos);
                if(distance > _range) continue;
                
                // interactable has the correct angle
                Vector3 toTarget = interactPos - ownPos;
                float angle = Vector3.Dot(_pov.forward, toTarget.normalized) / 2 + 0.5f;
                if(angle > _angle) continue;

                // we only want the closest interactable
                if (angle > closestAngle) continue;
                closestAngle = angle;
                CurrentInteractable = interactable;
            }
        }

        private void OnDrawGizmos() {
            if(CurrentInteractable == null) return;
            Gizmos.DrawLine(transform.position, CurrentInteractable.transform.position);
        }
    }
}