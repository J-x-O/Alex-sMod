using System;
using UnityEngine;

namespace AlexMod.Input {
    public class MovementSourcePlayer : MonoBehaviour, IMovementSource {

        [SerializeField] private MovementSystem _controller;
        
        private const float Threshold = 0.001f;
        
        private void Awake() => _controller.RegisterMovementSource(this);

        public MoveData GatherInputSnapshot() {
            Vector2 input = PlayerInputManager.Asset.Player.Move.ReadValue<Vector2>();
            bool jumping = PlayerInputManager.Asset.Player.Jump.IsPressed();
            
            // sending default saves bandwidth
            return input.magnitude > Threshold
                ? new MoveData(input, jumping)
                : default;
        }
    }
}