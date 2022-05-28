using System;
using AlexMod.Input;
using GameProgramming.Movement;
using UnityEngine;

namespace AlexMod.Movement.Behaviour {
    
    [Serializable]
    public class MovementBehaviourJumping : MovementBehaviour {

        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private float _jumpForce = 50f;
        [SerializeField] private float _jumpCooldown = 0.2f;

        private float _lastJumpTime;
        
        public override void HandleMovement(MoveData data, bool asServer, bool replaying = false) {
            
            // check if we have input and if the cooldown is over
            if(!data.Jumping) return;
            if(_lastJumpTime > Time.time) return;
            if(!_groundCheck.Grounded) return;

            _lastJumpTime = Time.time + _jumpCooldown;
            _rigidbody.AddForce(Vector3.up * _jumpForce);
        }
    }
}