using System;
using System.Threading.Tasks;
using BigD.Config;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.TimeManagement;
using Particles;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Huds.Scripts
{
    public class ArrestHud : MonoBehaviour
    {
        [SerializeField] private Image _progressBar;
        [SerializeField] private Transform _imageRoot;
        private Action _onComplete;
        private SignalBus _signalBus;
        
        private float _progress;
        private float _duration;
        private bool _autoCompletion;
        private ParticleManager _particleManager;

        [Inject]
        private void Construct(SignalBus signalBus,ParticleManager particleManager)
        {
            _particleManager = particleManager;
            _signalBus = signalBus;
        }
        
        public void Init(Action onComplete)
        {
            _onComplete = onComplete;
        }
        
        
        private void TimeTick()
        {
            _progress += 1;
            UpdateVisual();
            if (_progress >= _duration)
            {
                Complete();
            }
        }
        
        private void UpdateVisual()
        {
            _progressBar.DOFillAmount(_progress / _duration, 1).SetEase(Ease.Linear);
        }
        
        private async UniTask Complete()
        {
            _signalBus.TryUnsubscribe<TimeTickSignal>(TimeTick);
            await UniTask.Delay(TimeSpan.FromSeconds(1)); // TODO change delay for animation

            return;
            
            if (_autoCompletion)
            {
                
                Deactivate();
            }
        }

        public void SetActive(bool isShow)
        {
            gameObject.SetActive(isShow);
        }

        public void Deactivate()
        {
            _signalBus.TryUnsubscribe<TimeTickSignal>(TimeTick);
            _duration = 0;
            _progress = 0;
        }

        public void Activate(float duration,bool autoCompletion = true)
        {
            transform.localScale = Vector3.zero;
            _imageRoot.localScale = Vector3.zero;
            
            StartAnimation();
            _autoCompletion = autoCompletion;
            _signalBus.TryUnsubscribe<TimeTickSignal>(TimeTick);
            _duration = duration;
            _progress = 0;
            _progressBar.fillAmount = 0;
            _signalBus.Subscribe<TimeTickSignal>(TimeTick);
        }

        private async UniTask StartAnimation()
        {
            var startPos = new Vector3(0, 0f, -1.55f);
            var vector = transform.localPosition;
            var seq = DOTween.Sequence();
            seq.Append(transform.DOScale(Vector3.one, 0.5f));
            seq.Join(transform.DOLocalMove(vector, 0.5f).From(startPos));
            await seq.AsyncWaitForCompletion();
            await _imageRoot.DOScale(Vector3.one * 1.2f, 0.5f).SetEase(Ease.InOutElastic).AsyncWaitForCompletion();
            _imageRoot.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear);
        }
    }
}