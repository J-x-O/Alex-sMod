using System;
using AlexMod.Input;
using UnityEngine;

namespace AlexMod.Movement.Behaviour {
    
    [Serializable]
    public class MovementBehaviourRotate : MovementBehaviour {
       
        [SerializeField] private Transform _pov;
        
        public override void HandleMovement(MoveData data, bool asServer, bool replaying = false) {
            _rigidbody.MoveRotation(_transform.rotation * Quaternion.AngleAxis(data.Look.x, Vector3.up));
            
            Vector3 angles = _pov.eulerAngles;
            angles.x -= data.Look.y;

            // angle jumps to 360 when going below 0
            angles.x = angles.x > 180
                ? Mathf.Max(angles.x, 270f)
                : Mathf.Min(angles.x, 90);

            _pov.eulerAngles = angles;
        }
    }
}