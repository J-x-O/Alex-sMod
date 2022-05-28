using System;
using System.Collections;
using AlexMod.Utility;
using Beta.Devtools;
using FishNet.Managing.Timing;
using UnityEngine;

namespace GameProgramming.Movement {
    
    public class GroundCheck : NetworkBehaviourTimeSubscribtion {

        /// <summary> An Event that will be thrown, as soon as ground is initially touched </summary>
        public event Action OnGrounded;

        public event Action OnUngrounded;
        
        /// <summary> If the Object is Currently touching the Ground </summary>
        public bool Grounded { get; private set; }
        
        [Tooltip("The OriginPoint from where we will do the ground check")]
        [SerializeField] private Optional<Transform> _target;

        [Tooltip("The Dimensions of the Box, we will always extend downwards using the Y-Axis")]
        [SerializeField] private Vector3 _dimensions;

        private Coroutine _refreshCoroutine;
        
        private Vector3 _hitBoxCenter
            => (_target.Enabled ? _target.Value.position : transform.position)
               - new Vector3(0,_dimensions.y / 2,0);

        /// <summary> Visualize the GroundCheck for Debugging </summary>
        public void OnDrawGizmos() {
            if (!Application.isPlaying) RefreshGrounded();
            Gizmos.color = Grounded ?  Color.green : Color.red;
            Gizmos.DrawCube(_hitBoxCenter, _dimensions);
        }
        
        protected override void HandleSubscription(TimeManager manager, bool subscribe) {
            if (subscribe) manager.OnPostTick += RefreshGrounded;
            else manager.OnPostTick -= RefreshGrounded;
        }

        private void RefreshGrounded() {
            int mask = LayerMask.GetMask("Ground");

            Collider[] hits = Physics.OverlapBox(_hitBoxCenter, _dimensions / 2, transform.rotation, mask);

            //If we are switching from ungrounded to grounded, throw the Event
            if (hits.Length > 0) {
                bool oldValue = Grounded;
                Grounded = true;
                if (oldValue != Grounded) OnGrounded?.Invoke();
            }
            //If we are switching from grounded to ungrounded, throw the Event
            else {
                bool oldValue = Grounded;
                Grounded = false;
                if (oldValue != Grounded) OnUngrounded?.Invoke();
                
            }
        }
    }
}