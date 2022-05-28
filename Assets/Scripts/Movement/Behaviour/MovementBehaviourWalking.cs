using System;
using AlexMod.Input;
using UnityEngine;

namespace AlexMod.Movement.Behaviour {
    
    [Serializable]
    public class MovementBehaviourWalking : MovementBehaviour {
        
        [SerializeField] private float _moveForce = 30f;
        [SerializeField] private float _maxSpeed = 20;

        public override void HandleMovement(MoveData data, bool asServer, bool replaying = false) {

            // calculate the relative movement direction
            Vector3 force = new Vector3(data.Movement.x, 0f, data.Movement.y) * _moveForce;
            force = _rigidbody.rotation * force;
            
            if (_rigidbody.velocity.magnitude > _maxSpeed) {
                Vector3 velo = _rigidbody.velocity;
                velo.y = 0;
                
                // check how far the input differs from the current direction 
                float strength = Vector3.Dot(velo.normalized, force.normalized);
                strength = 1 - (strength / 2 + 0.5f);
                force *= strength;
            }

            _rigidbody.AddForce(force);
            
        }
    }
}