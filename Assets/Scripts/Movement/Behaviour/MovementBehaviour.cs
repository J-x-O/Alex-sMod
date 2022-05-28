using System;
using AlexMod.Input;
using UnityEngine;

namespace AlexMod.Movement.Behaviour {
    
    [Serializable]
    public abstract class MovementBehaviour {
        
        protected MovementSystem _system;
        protected Transform _transform;
        protected Rigidbody _rigidbody;

        public void Initialize(MovementSystem system, Rigidbody rigidbody) {
            _system = system;
            _transform = system.transform;
            _rigidbody = rigidbody;
        }

        public abstract void HandleMovement(MoveData data, bool asServer, bool replaying = false);

    }
}