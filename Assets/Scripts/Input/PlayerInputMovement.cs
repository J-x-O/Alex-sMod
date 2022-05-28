using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AlexMod.Input {
    public class PlayerInputMovement : MonoBehaviour, IMovementSource {

        [SerializeField] private MovementSystem _controller;
        
        private const float Threshold = 0.001f;
        
        private void Awake() => _controller.RegisterMovementSource(this);

        public MoveData GatherInputSnapshot() {
            Vector2 movement = PlayerInputManager.Asset.Player.Move.ReadValue<Vector2>();
            Vector2 look = PlayerInputManager.Asset.Player.Look.ReadValue<Vector2>();
            bool jumping = PlayerInputManager.Asset.Player.Jump.IsPressed();

            // sending default saves bandwidth
            return movement.magnitude > Threshold || look.magnitude > Threshold || jumping
                ? new MoveData(movement, look, jumping)
                : default;
        }
    }
}