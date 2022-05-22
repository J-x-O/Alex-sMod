using System;
using UnityEngine;

namespace JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween {
    public class GenericAction {
        
        private Action _baseAction;
        private Action<float> _floatAction;
        private Action<Vector3> _vector3Action;

        public void Invoke() {
            _baseAction?.TryInvoke();
            _floatAction?.TryInvoke(0);
            _vector3Action?.TryInvoke(Vector3.zero);
            
        }

        public void Invoke(float value) {
            _baseAction?.TryInvoke();
            _floatAction?.TryInvoke(value);
            _vector3Action?.TryInvoke(new Vector3(value, value, value));
        }
        
        public void Invoke(float floatValue, Vector3 vecValue) {
            _baseAction?.TryInvoke();
            _floatAction?.TryInvoke(floatValue);
            _vector3Action?.TryInvoke(vecValue);
        }

        private void ResetAllActions() {
            _baseAction = null;
            _floatAction = null;
            _vector3Action = null;
        }
        
        public void SetAction(Action action) {
            ResetAllActions();
            _baseAction = action;
        }

        public void SetAction(Action<float> action) {
            ResetAllActions();
            _floatAction = action;
        }
        
        public void SetAction(Action<Vector3> action) {
            ResetAllActions();
            _vector3Action = action;
        }
    }
}