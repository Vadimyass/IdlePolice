using System;
using BigD.Config;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Configs;
using Gameplay.Scripts.Locker;
using Gameplay.Scripts.TimeManagement;
using Gameplay.Scripts.Utils;
using Particles;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Huds.Scripts
{
    public class CrimesHud : MonoBehaviour
    {
        [SerializeField] private Image _progressBar;
        [SerializeField] private Transform _particlePoofRoot;
        [SerializeField] private Transform _imageRoot;
        [SerializeField] private Transform _crossTransform;
        private Action _onComplete;
        private SignalBus _signalBus;

        private float _progress;
        private float _duration;
        private GameConfig _gameConfig;
        private ParticleManager _particleManager;
        private LockController _lockController;

        [Inject]
        private void Construct(SignalBus signalBus, LockController lockController, GameConfig gameConfig,ParticleManager particleManager)
        {
            _lockController = lockController;
            _particleManager = particleManager;
            _gameConfig = gameConfig;
            _signalBus = signalBus;
        }
        public void Init(Action onComplete)
        {
            _signalBus.TryUnsubscribe<TimeTickSignal>(TimeTick);
            _onComplete = onComplete;
            _progressBar.fillAmount = 1;
            _signalBus.Subscribe<TimeTickSignal>(TimeTick);
        }

        private void TimeTick()
        {
            if (_lockController.HaveLock<OrderTickLocker>())
            {
                return;
            }
            
            _progress -= 1;
            UpdateVisual();
            if (_progress <= 0)
            {
                Complete();
            }
        }

        private void UpdateVisual()
        {
            _progressBar.DOFillAmount(_progress / _duration, 1).SetEase(Ease.Linear);
        }

        public void Activate()
        {
            _particleManager.PlayParticleInPosition(ParticleType.poof_red,_particlePoofRoot.position,Quaternion.identity,20);
            transform.localScale = Vector3.zero;
            _imageRoot.localScale = Vector3.zero;
         
            _crossTransform.localScale = Vector3.zero;


            StartAnimation();
            
            _duration = _gameConfig.CriminalsConfig.GetCriminal().CriminalTime;
            _progress = _duration;
            _progressBar.fillAmount = 1;
        }

        private async void StartAnimation()
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

        public void Deactivate()
        {
            _signalBus.TryUnsubscribe<TimeTickSignal>(TimeTick);
            _duration = 0;
            _progress = 0;
        }

        private async void Complete()
        {
            _onComplete();
            _signalBus.TryUnsubscribe<TimeTickSignal>(TimeTick);
            await _imageRoot.DOScale(0, 0.5f).From(1).AsyncWaitForCompletion();
            await _crossTransform.DOScale(2.5f, 0.3f).From(0).AsyncWaitForCompletion();
            await _crossTransform.DOScale(2f, 0.2f).AsyncWaitForCompletion();
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        }
        
        public void SetActive(bool isShow)
        {
            gameObject.SetActive(isShow);
            if (isShow == true)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }
    }
}