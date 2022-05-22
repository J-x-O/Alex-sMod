using System;
using System.Linq;
using UnityEngine;

namespace JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween {
    public static class EventExtensions {

        public static void TryInvoke(this Action action) {
            
            if (action == null) return;

            foreach(Action handler in action.GetInvocationList().Cast<Action>()) {
                try {
                    handler();
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        }
        
        public static void TryInvoke<T>(this Action<T> action, T param) {
            
            if (action == null) return;

            foreach(Action<T> handler in action.GetInvocationList().Cast<Action<T>>()) {
                try {
                    handler(param);
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        }
        
        public static void TryInvoke<T, TA>(this Action<T, TA> action, T param, TA secondParam) {
            
            if (action == null) return;

            foreach(Action<T, TA> handler in action.GetInvocationList().Cast<Action<T, TA>>()) {
                try {
                    handler(param, secondParam);
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        }
        
    }
}