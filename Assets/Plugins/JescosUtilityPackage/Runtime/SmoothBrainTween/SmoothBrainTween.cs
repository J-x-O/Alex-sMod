using System.Collections.Generic;
using UnityEngine;

namespace JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween {
    public partial class SmoothBrainTween : MonoBehaviour {

        // the smooth brain instance that handles all tweens running
        private static SmoothBrainTween _instance => _internalInstance ??= AutoGenerateScriptInstance();
        private static SmoothBrainTween _internalInstance;

        /// <summary> </summary>
        private Dictionary<TweenInfo, Coroutine> _runningCoroutines = new Dictionary<TweenInfo, Coroutine>();
        
        public enum UpdateMode { EveryFrame, FixedUpdate }
        public static UpdateMode TweenUpdateMode { get; set; } = UpdateMode.EveryFrame;

        private static SmoothBrainTween AutoGenerateScriptInstance() {
            GameObject target = new GameObject("SmoothBrainTween");
            DontDestroyOnLoad(target);
            return target.AddComponent<SmoothBrainTween>();
        }

        private void AddNewTween(TweenInfo tween, Coroutine routine) {
            _runningCoroutines.Add(tween, routine);
        }

        /// <summary> Stops the provided Tween from running any further </summary>
        /// <param name="tween"> the tween you want to stop </param>
        public static void Cancel(TweenInfo tween) {
            if (tween == null || tween.Running == false) return;
            if (!_instance._runningCoroutines.ContainsKey(tween)) return;
            tween.Running = false;
        }

        private void RemoveTweenInternal(TweenInfo tween) {
            if (!_runningCoroutines.ContainsKey(tween)) {
                Debug.LogError("SmoothBrainTween Internal Error: Couldn't remove Tween");
                return;
            }
            tween.Running = false;
            _runningCoroutines.Remove(tween);
        }
    }
}