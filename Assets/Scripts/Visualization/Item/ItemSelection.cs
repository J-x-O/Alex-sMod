using System;
using JescoDev.Utility.SmoothBrainTween.Plugins.Runtime.SmoothBrainTween;
using UnityEngine;

namespace AlexMod.Visualization.Item {
    public class ItemSelection : MonoBehaviour {

        [SerializeField] private Inventory.Items.Item _item;
        [SerializeField] private GameObject _visualization;
        [SerializeField] private float _selectDuration = 0.2f;
        [SerializeField] private float _selectAngle = -90f;
        [SerializeField] private float _useDuration = 0.2f;
        [SerializeField] private float _useAngle = 45f;

        private TweenInfo _tween;
        
        private void OnEnable() {
            _item.OnItemPickUp += HandlePickUp;
            _item.OnItemDrop += HandleDrop;
            _item.OnItemSelected += HandleSelected;
            _item.OnItemUnselected += HandleUnselected;
            _item.OnItemUsed += HandleUse;
        }

        private void HandlePickUp() => _visualization.SetActive(false);
        
        private void HandleDrop() {
            SmoothBrainTween.Cancel(_tween);
            _visualization.transform.localRotation = Quaternion.identity;
            _visualization.SetActive(true);
        }
        
        private void HandleSelected() {
            SmoothBrainTween.Cancel(_tween);
            _visualization.transform.localRotation = Quaternion.AngleAxis(_selectAngle, Vector3.left);
            _tween = SmoothBrainTween.Rotate(_visualization, Quaternion.identity, _selectDuration)
                .SetEaseSinOut();
            _visualization.SetActive(true);
        }
        
        private void HandleUnselected() {
            SmoothBrainTween.Cancel(_tween);
            _visualization.SetActive(false);
        }

        private void HandleUse() {
            SmoothBrainTween.Cancel(_tween);
            _visualization.transform.localRotation = Quaternion.identity;
            Quaternion target = Quaternion.AngleAxis(_useAngle, Vector3.left);
            _tween = SmoothBrainTween.Rotate(_visualization, target, _useDuration)
                .SetEaseSinOut()
                .SetLoopPingPong(1);
        }
    }
}