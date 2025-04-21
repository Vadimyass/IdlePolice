/*using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.Advertisement;
using Gameplay.Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class RewardAdButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _loadingImage;
        [SerializeField] private Transform _loadingTransform;
        private AdManager _adManager;

        [Inject]
        private void Construct(AdManager adManager)
        {
            _adManager = adManager;
        }

        public void AddButtonListener(UnityAction call)
        {
            RemoveAllListeners();
            _button.onClick.AddListener(call);
            _loadingImage.transform.DORotate(Vector3.forward * 360, 1, RotateMode.FastBeyond360)
                .SetLoops(-1).SetEase(Ease.Linear).SetUpdate(true);
        }

        public async void Show()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnAdLoad;
            _button.interactable = false;
            _button.gameObject.SetActive(false);
            _loadingTransform.gameObject.SetActive(true);

            _loadingTransform.DOScale(1, 0.25f).From(0).SetEase(Ease.OutBack).SetUpdate(true);
            
            if(_adManager.IsRewardedLoad() == false)
            {
                MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnAdLoad;
            
                _adManager.LoadRewardedAd();
            }
            else
            {
                OnAdLoad();
            }
        }
        

        private void OnAdLoad(string arg1, MaxSdkBase.AdInfo arg2)
        {
            OnAdLoad();
        }

        public void SetInteractable(bool interactable)
        {
            _button.interactable = interactable;
        }
        
        private async void OnAdLoad()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), true);
            
            
            var seq = DOTween.Sequence();
            seq.Append(_loadingTransform.DOScale(0, 0.25f).From(1).SetEase(Ease.OutCubic).SetUpdate(true));
            seq.AppendCallback((() =>
            {
                _button.gameObject.SetActive(true);
            }));
            seq.Append(_button.transform.DOScale(1, 0.25f).From(0).SetEase(Ease.OutBack).SetUpdate(true));

            seq.SetUpdate(true);
            await seq.AsyncWaitForCompletion();

            _button.interactable = true;
        }
        
        
        public void RemoveAllListeners()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}*/