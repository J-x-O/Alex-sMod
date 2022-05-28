using System;
using Cinemachine;
using FishNet.Object;
using UnityEngine;

namespace AlexMod.Camera {
    public class CameraController : NetworkBehaviour {

        public enum CameraMode { FirstPerson, ThirdPerson }

        [SerializeField] private CameraMode _initialCameraMode;
        
        [SerializeField] private CinemachineVirtualCamera _firstPerson;
        [SerializeField] private CinemachineVirtualCamera _thirdPerson;

        private void Awake() {
            _firstPerson.gameObject.SetActive(false);
            _thirdPerson.gameObject.SetActive(false);
        }

        public override void OnStartClient() {
            base.OnStartClient();
            if (IsOwner) {
                SwitchCameraMode(_initialCameraMode);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public void SetEyeLevel(float eyeLevel) => transform.position = new Vector3(0,eyeLevel,0);

        public void SwitchCameraMode(CameraMode mode) {
            if (!IsOwner) return;
            _firstPerson.gameObject.SetActive(mode == CameraMode.FirstPerson);
            _thirdPerson.gameObject.SetActive(mode == CameraMode.ThirdPerson);
        }
    }
}