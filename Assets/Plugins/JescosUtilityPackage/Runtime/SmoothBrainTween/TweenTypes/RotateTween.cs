using UnityEngine;

namespace JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween {
    public partial class SmoothBrainTween {
        
        public static TweenInfo Rotate(GameObject gameObject, Quaternion targetRotation, float duration) {
            TweenInfo info = new TweenInfo();

            Quaternion startRotation = gameObject.transform.localRotation;
            
            Coroutine routine = _instance.StartCoroutine(
                GenericTweenRoutineCoroutine(info, duration, null ,
                    progress => ApplyMove(progress, gameObject, startRotation, targetRotation),
                    progress => ApplyMove(progress, gameObject, startRotation, targetRotation)));

            _instance.AddNewTween(info, routine);
            return info;
        }

        private static void ApplyMove(float progress, GameObject gameObject, Quaternion startRotation, Quaternion targetRotation) {
            Quaternion currentRotation = Quaternion.Lerp(startRotation, targetRotation, progress);
            gameObject.transform.localRotation = currentRotation;
        }
        
    }
}