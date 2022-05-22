using System;
using System.Collections;
using UnityEngine;

namespace JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween {
    public partial class SmoothBrainTween {
        
        public static TweenInfo DelayedCall(float duration, Action action) {
            TweenInfo info = new TweenInfo();
            info.SetOnFinish(action);

            Coroutine routine = _instance.StartCoroutine(DelayedCallCoroutine(info, duration));
            _instance.AddNewTween(info, routine);
            return info;
        }

        private static IEnumerator DelayedCallCoroutine(TweenInfo info, float duration) {
            // silent advance 1 step to wait for the events to subscribe
            TimeTracker time = new TimeTracker(info, duration);
            yield return new WaitForEndOfFrame();

            while (time.Running && !info.Running) {
                info._onUpdate.Invoke(time.PercentProgress);
                yield return time.Advance();
            }

            info._onFinish?.Invoke();
            _instance.RemoveTweenInternal(info);
        }
        
    }
}