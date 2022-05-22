using System;
using UnityEngine;

namespace JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween {
    public partial class TweenInfo {

        /// <summary> The Function defining the easing type </summary>
        public Func<float, float> Easing { get; private set; } = x => x;
        
        //just a collection of math stolen from https://easings.net/#

        public TweenInfo SetEaseLinear() => FuncWrapper(x => x);
        
        public TweenInfo SetEaseSinIn() => FuncWrapper(x => 1 - Mathf.Cos((x * Mathf.PI) / 2));
        public TweenInfo SetEaseSinOut() => FuncWrapper(x => Mathf.Sin((x * Mathf.PI) / 2));
        public TweenInfo SetEaseSinInOut() => FuncWrapper(x => -(Mathf.Cos(Mathf.PI * x) - 1) / 2);
        
        public TweenInfo SetEaseQuadIn() => FuncWrapper(x => Mathf.Pow(x, 2));
        public TweenInfo SetEaseQuadOut() => FuncWrapper(x => 1 - (1 - x) * (1 - x));
        public TweenInfo SetEaseQuadInOut() => FuncWrapper(x => x < 0.5
            ? 2 * x * x
            : 1 - Mathf.Pow(-2 * x + 2, 2) / 2);
        
        public TweenInfo SetEaseCubicIn() => FuncWrapper(x => Mathf.Pow(x, 3));
        public TweenInfo SetEaseCubicOut() => FuncWrapper(x => 1 - Mathf.Pow(1 - x, 3));
        public TweenInfo SetEaseCubicInOut() => FuncWrapper(x => x < 0.5
            ? 4 * Mathf.Pow(x, 3)
            : 1 - Mathf.Pow(-2 * x + 2, 3) / 2);
        
        public TweenInfo SetEaseQuartIn() => FuncWrapper(x => Mathf.Pow(x, 4));
        public TweenInfo SetEaseQuartOut() => FuncWrapper(x => 1 - Mathf.Pow(1 - x, 4));
        public TweenInfo SetEaseQuartInOut() => FuncWrapper(x => x < 0.5
            ? 8 * Mathf.Pow(x, 4)
            : 1 - Mathf.Pow(-2 * x + 2, 4) / 2);
        
        public TweenInfo SetEaseQuintIn() => FuncWrapper(x => Mathf.Pow(x, 5));
        public TweenInfo SetEaseQuintOut() => FuncWrapper(x => 1 - Mathf.Pow(1 - x, 5));
        public TweenInfo SetEaseQuintInOut() => FuncWrapper(x => x < 0.5
            ? 16 * Mathf.Pow(x, 5)
            : 1 - Mathf.Pow(-2 * x + 2, 5) / 2);

        public TweenInfo SetEaseExpoIn() => FuncWrapper(x => Fallback0(x, () => Mathf.Pow(2, 10 * x - 10)));
        public TweenInfo SetEaseExpoOut() => FuncWrapper(x => Fallback1(x, () => 1 - Mathf.Pow(2, -10 * x)));
        public TweenInfo SetEaseExpoInOut() => FuncWrapper(x => Fallback01(x, () => x < 0.5
            ? Mathf.Pow(2, 20 * x - 10) / 2
            : (2 - Mathf.Pow(2, -20 * x + 10)) / 2));
  
        public TweenInfo SetEaseCircIn() => FuncWrapper(x => 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2)));
        public TweenInfo SetEaseCircOut() => FuncWrapper(x => Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2)));
        public TweenInfo SetEaseCircInOut() => FuncWrapper(x => x < 0.5
            ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
            : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2);

        private const float BackConstant1 = 1.70158f;
        private const float BackConstant2 = BackConstant1 * 1.525f;
        private const float BackConstant3 = BackConstant1 + 1f;
        public TweenInfo SetEaseBackIn() => FuncWrapper(x => BackConstant3 * Mathf.Pow(x, 3) - BackConstant1 * Mathf.Pow(x, 2));
        public TweenInfo SetEaseBackOut() => FuncWrapper(x => 1 + BackConstant3 * Mathf.Pow(x - 1, 3) + BackConstant1 * Mathf.Pow(x - 1, 2));
        public TweenInfo SetEaseBackInOut() => FuncWrapper(x => x < 0.5
            ? (Mathf.Pow(2 * x, 2) * ((BackConstant2 + 1) * 2 * x - BackConstant2)) / 2
            : (Mathf.Pow(2 * x - 2, 2) * ((BackConstant2 + 1) * (x * 2 - 2) + BackConstant2) + 2) / 2);

        private const float ElasticConstant1 = (2 * Mathf.PI) / 3;
        private const float ElasticConstant2 = (2 * Mathf.PI) / 4.5f;
        public TweenInfo SetEaseElasticIn() => FuncWrapper(x => Fallback01(x, () => -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * ElasticConstant1)));
        public TweenInfo SetEaseElasticOut() => FuncWrapper(x => Fallback01(x, () => Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * ElasticConstant1) + 1));
        public TweenInfo SetEaseElasticInOut() => FuncWrapper(x => Fallback01(x, () => x < 0.5
            ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * ElasticConstant2)) / 2
            : (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * ElasticConstant2)) / 2 + 1));

        private float BounceFunction(float x) {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            if (x < 1 / d1) return n1 * x * x;
            if (x < 2 / d1) return n1 * (x -= 1.5f / d1) * x + 0.75f;
            if (x < 2.5 / d1) return n1 * (x -= 2.25f / d1) * x + 0.9375f;
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
        public TweenInfo SetEaseBounceIn() => FuncWrapper(x => 1 - BounceFunction(1 - x));
        public TweenInfo SetEaseBounceOut() => FuncWrapper(BounceFunction);
        public TweenInfo SetEaseBounceInOut() => FuncWrapper(x => x < 0.5
            ? (1 - BounceFunction(1 - 2 * x)) / 2
            : (1 + BounceFunction(2 * x - 1)) / 2);

        public TweenInfo SetCustomEase(AnimationCurve curve) => FuncWrapper(curve.Evaluate);

        // fallback functions to return a value instead of computing the other one
        private float Fallback0(float x, Func<float> otherValue) {
            if (Math.Abs(x) < 0.00001f) return 0;
            return otherValue();
        }
        
        private float Fallback1(float x, Func<float> otherValue) {
            if (Math.Abs(x - 1) < 0.00001f) return 1;
            return otherValue();
        }
        
        private float Fallback01(float x, Func<float> otherValue) {
            if (Math.Abs(x) < 0.00001f) return 0;
            if (Math.Abs(x - 1) < 0.00001f) return 1;
            return otherValue();
        }

        /// <summary> utility function to set the easing and return the info object for chaining syntax </summary>
        private TweenInfo FuncWrapper(Func<float, float> function) {
            Easing = function;
            return this;
        }
    }
}