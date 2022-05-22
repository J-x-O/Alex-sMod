using UnityEngine;

namespace JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween {
    public partial class SmoothBrainTween {
        
        public static TweenInfo Scale(GameObject gameObject, Vector3 targetScale, float duration) {
            TweenInfo info = new TweenInfo();
            
            Vector3 startScale = gameObject.transform.localScale;
            
            Coroutine routine = _instance.StartCoroutine(
                GenericTweenRoutineCoroutine(info, duration, null ,
                    progress => ApplyScale(progress, gameObject, startScale, targetScale),
                    progress => ApplyScale(progress, gameObject, startScale, targetScale),
                    customVecValue: f => Vector3.Lerp(startScale, targetScale, f)));
            
            _instance.AddNewTween(info, routine);
            return info;
        }

        private static void ApplyScale(float progress, GameObject gameObject, Vector3 startScale, Vector3 targetScale) {
            Vector3 currentScale = Vector3.Lerp(startScale, targetScale, progress);
            gameObject.transform.localScale = currentScale;
        }

    }
}