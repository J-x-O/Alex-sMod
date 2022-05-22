using System;
using System.Collections;
using UnityEngine;

namespace JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween {
    public partial class SmoothBrainTween {

        private static IEnumerator GenericTweenRoutineCoroutine(TweenInfo info, float duration, 
            Action setup = null, Action<float> update = null, Action<float> finalize = null,
            Func<float, float> remapValue = null, Func<float, Vector3> customVecValue = null) {

            remapValue ??= f => f;
            customVecValue ??= f => new Vector3(f,f,f);
            
            // silent advance 1 step to wait for the user events to subscribe
            TimeTracker time = new TimeTracker(info, duration);
            yield return new WaitForEndOfFrame();
            
            setup?.Invoke();

            while (time.Running && !info.Running) {
                float progress = info.Easing(time.PercentProgress);
                float remappedProgress = remapValue.Invoke(progress);
                InvokeCancelOnError(info, () => update?.Invoke(remappedProgress));
                info._onUpdate.Invoke(remappedProgress, customVecValue.Invoke(progress));
                yield return time.Advance();
            }
            
            InvokeCancelOnError(info, () => finalize?.Invoke(time.PercentProgress));
            info._onFinish?.Invoke(remapValue.Invoke(1), customVecValue.Invoke(1));
            _instance.RemoveTweenInternal(info);
        }
        
        // if anything fails while tweening just cancel the tween
        private static  void InvokeCancelOnError(TweenInfo info, Action target) {
            try { target.Invoke(); }
            catch (Exception e) { Cancel(info); }
        }
    }
}