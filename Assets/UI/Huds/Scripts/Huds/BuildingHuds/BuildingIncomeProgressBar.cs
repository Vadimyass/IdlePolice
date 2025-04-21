using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.TimeManagement;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Huds.Scripts.BuildingHuds
{
    public class BuildingIncomeProgressBar : HudBase
    {
        [SerializeField] private Image _progressImage;

        public override void Init()
        {
            _progressImage.fillAmount = 0;
            Hide();
        }

        
        public void Configurate(double incomeProgress)
        {
            Show();
            UpdateInfo(incomeProgress);
        }
        
        public async void UpdateInfo(double incomeProgress, bool withDelay = false)
        {
            
            if (withDelay == false)
            {
                _progressImage.fillAmount = (float)incomeProgress;
                return;
            }
            
            if (incomeProgress < _progressImage.fillAmount)
            {
                if (incomeProgress <= 0.1f)
                {
                    await _progressImage.DOFillAmount(1, 0.99f).AsyncWaitForCompletion();
                    _progressImage.fillAmount = (float)incomeProgress;
                    return;
                }
                
                var seq = DOTween.Sequence();
                var timeForFull = (1 - _progressImage.fillAmount) / (1 - (float)incomeProgress);
                seq.Append(_progressImage.DOFillAmount(1, timeForFull));
                seq.Append(_progressImage.DOFillAmount((float)incomeProgress, 1-timeForFull));
                return;
            }
            _progressImage.DOFillAmount((float)incomeProgress, 1f);
        }

        protected override UniTask AnimationIn()
        {
            throw new System.NotImplementedException();
        }

        protected override UniTask AnimationOut()
        {
            throw new System.NotImplementedException();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (this != null)
            {
                gameObject.SetActive(false);
            }
        }
    }
}