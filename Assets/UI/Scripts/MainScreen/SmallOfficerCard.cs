using System;
using DG.Tweening;
using Pool;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.MainScreen
{
    public class SmallOfficerCard : MonoBehaviour, IPoolable
    {
        [SerializeField] private Image _image;
        [SerializeField] private Transform _shinyTransform;
        private Sequence _sequence;

        private void Awake()
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(_shinyTransform.DORotate(Vector3.forward * (_shinyTransform.rotation.eulerAngles.z+360), 2f,RotateMode.FastBeyond360).SetEase(Ease.Linear));
            _sequence.SetLoops(-1);
        }

        public void Show(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void Return()
        {
            
        }

        public void Release()
        {
            
        }
    }
}