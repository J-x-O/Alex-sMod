using System;
using System.Collections;
using Beta.Devtools;
using UnityEngine;

namespace GameProgramming.Movement {
    
    public class GroundCheck : MonoBehaviour {

        /// <summary> An Event that will be thrown, as soon as ground is initially touched </summary>
        public event Action OnGrounded;

        public event Action OnUngrounded;
        
        /// <summary> If the Object is Currently touching the Ground </summary>
        public bool Grounded { get; private set; }
        
        [Tooltip("The OriginPoint from where we will do the ground check")]
        [SerializeField] private Optional<Transform> _target;

        [Tooltip("The Dimensions of the Box, we will always extend downwards using the Y-Axis")]
        [SerializeField] private Vector3 _dimensions;

        [Tooltip("How often we will refresh the GroundCheck")]
        [SerializeField] private float _refreshRate = 0.1f;

        private Coroutine _refreshCoroutine;
        
        private Vector3 _hitBoxCenter
            => (_target.Enabled ? _target.Value.position : transform.position)
               - new Vector3(0,_dimensions.y / 2,0);

        private void OnEnable() => _refreshCoroutine = StartCoroutine(RefreshCoroutine());
        
        private void OnDisable() => StopCoroutine(_refreshCoroutine);

        /// <summary> Visualize the GroundCheck for Debugging </summary>
        public void OnDrawGizmos() {
            if (!Application.isPlaying) RefreshGrounded();
            Gizmos.color = Grounded ?  Color.green : Color.red;
            Gizmos.DrawCube(_hitBoxCenter, _dimensions);
        }

        private IEnumerator RefreshCoroutine() {
            while (isActiveAndEnabled) {
                RefreshGrounded();
                yield return new WaitForSeconds(_refreshRate);
            }
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