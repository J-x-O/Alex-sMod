using System.Collections;
using UnityEngine;

namespace JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween {
    
    public partial class SmoothBrainTween {
        
        public static TweenInfo Value(float startValue, float targetValue, float duration) {
            TweenInfo info = new TweenInfo();
            Coroutine routine = _instance.StartCoroutine(
                GenericTweenRoutineCoroutine(info, duration, 
                    remapValue: f => Mathf.Lerp(startValue, targetValue, f)));
            
            _instance.AddNewTween(info, routine);
            return info;
        }

        public static TweenInfo Value(Vector3 startValue, Vector3 targetValue, float duration) {
            TweenInfo info = new TweenInfo();
            Coroutine routine = _instance.StartCoroutine(
                GenericTweenRoutineCoroutine(info, duration, 
                    customVecValue: f => Vector3.Lerp(startValue, targetValue, f)));
            
            _instance.AddNewTween(info, routine);
            return info;
        }
    }
}