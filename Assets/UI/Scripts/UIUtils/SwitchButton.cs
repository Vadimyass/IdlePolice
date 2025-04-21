using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.UIUtils
{
    public class SwitchButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Transform _switcher;
        [SerializeField] private Transform _onPosition;
        [SerializeField] private Transform _offPosition;
        [SerializeField] private float _animationTime = 0.25f;
        private bool _isOn;
        private bool _isCanBeClicked = true;

        public void Init(bool isOn, UnityAction<bool> action)
        {
            _button.onClick.RemoveAllListeners();
            _isOn = isOn;
            
            
            if (_isOn == true)
            {
                _switcher.position = _onPosition.position;
                _backgroundImage.color = Color.green;
            }
            else
            {
                _switcher.position = _offPosition.position;
                _backgroundImage.color = Color.grey;
            }
            
            _button.onClick.AddListener((() =>
            {
                if(_isCanBeClicked == false) return;
                SwitchSwitcher();
                action(_isOn);
            }));
        }

        private async void SwitchSwitcher()
        {
            _isCanBeClicked = false;
            _isOn = !_isOn;
            if (_isOn == false)
            {
                _switcher.DOMove(_offPosition.position, _animationTime);
                _backgroundImage.color = Color.grey;
            }
            else
            {
                _switcher.DOMove(_onPosition.position, _animationTime);
                _backgroundImage.color = Color.green;
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(_animationTime));
            _isCanBeClicked = true;
        }
    }
}