using UnityEngine;

namespace JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween {
    public partial class SmoothBrainTween {
        
        public static TweenInfo Move(GameObject gameObject, Vector3 targetPosition, float duration) {
            TweenInfo info = new TweenInfo();

            Vector3 startPosition = gameObject.transform.position;
            
            Coroutine routine = _instance.StartCoroutine(
                GenericTweenRoutineCoroutine(info, duration, null ,
                    progress => ApplyMove(progress, gameObject, startPosition, targetPosition),
                    progress => ApplyMove(progress, gameObject, startPosition, targetPosition),
                    customVecValue: f => Vector3.Lerp(startPosition, targetPosition, f)));

            _instance.AddNewTween(info, routine);
            return info;
        }

        private static void ApplyMove(float progress, GameObject gameObject, Vector3 startPosition, Vector3 targetPosition) {
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, progress);
            gameObject.transform.position = currentPosition;
        }

    }
}