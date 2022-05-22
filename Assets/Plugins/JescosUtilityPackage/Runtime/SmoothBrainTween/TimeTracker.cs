using System;
using System.Collections;
using UnityEngine;

namespace JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween {
    public class TimeTracker {
        
        public bool Running { get; private set; }  = true;

        public float PercentProgress => _info.PingPong && CompletedLoops % 2 != 0
            ? 1 - _passedTime / _duration
            : _passedTime / _duration;

        public int CompletedLoops { get; private set; }
        
        private float _passedTime;
        private readonly float _duration;
        private readonly TweenInfo _info;

        public TimeTracker(TweenInfo info, float duration) {
            _info = info;
            _duration = duration;
        }

        public IEnumerator Advance(bool trackTime = true) {
            switch (SmoothBrainTween.TweenUpdateMode) {
                case SmoothBrainTween.UpdateMode.EveryFrame:
                    if (trackTime) _passedTime += Time.deltaTime;
                    yield return null;
                    break;
                case SmoothBrainTween.UpdateMode.FixedUpdate:
                    if (trackTime) _passedTime += Time.fixedDeltaTime;
                    yield return new WaitForFixedUpdate();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_duration < _passedTime) {
                CompletedLoops++;
                Running = CompletedLoops < _info.LoopCount;
                if(_info.PingPong || Running) _passedTime = 0;
            }
        }
    }
}