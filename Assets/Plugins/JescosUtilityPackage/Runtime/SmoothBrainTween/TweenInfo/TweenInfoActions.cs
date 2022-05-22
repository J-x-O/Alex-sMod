using System;
using UnityEngine;

namespace JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween {
    public partial class TweenInfo {
        
        /// <summary> True as long as the tween is still executing </summary>
        public bool Running { get; internal set; }
                
        /// <summary> The Action that will be invoked once the tween finishes executing </summary>
        internal readonly GenericAction _onFinish = new GenericAction();

        /// <summary> The Action that will be invoked every time the tween updates </summary>
        internal readonly GenericAction _onUpdate = new GenericAction();

        public TweenInfo SetOnUpdate(Action onUpdate) { _onUpdate.SetAction(onUpdate); return this; }
        public TweenInfo SetOnUpdate(Action<float> onUpdate) { _onUpdate.SetAction(onUpdate); return this; }
        public TweenInfo SetOnUpdate(Action<Vector3> onUpdate) { _onUpdate.SetAction(onUpdate); return this; }

        public TweenInfo SetOnFinish(Action onFinish) { _onFinish.SetAction(onFinish); return this; }
        public TweenInfo SetOnFinish(Action<float> onFinish) { _onFinish.SetAction(onFinish); return this; }
        public TweenInfo SetOnFinish(Action<Vector3> onFinish) { _onFinish.SetAction(onFinish); return this; }
    }
}