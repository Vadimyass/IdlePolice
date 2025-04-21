using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts._3DTextPoolManagement;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Utils;
using Particles;
using TMPro;
using UI.UIUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.Huds.Scripts.BuildingHuds
{
    public class BuildingIncomeText : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private TextMeshPro _incomeText;
        [SerializeField] private Transform _incomeTransform;

        private UnityAction _takeIncomeAction;
        private double _income;
        private ParticleManager _particleManager;
        private TextPoolManager _textPoolManager;


        [Inject]
        private void Construct(ParticleManager particleManager, TextPoolManager textPoolManager)
        {
            _textPoolManager = textPoolManager;
            _particleManager = particleManager;
        }
        
        public void Configurate(double income, UnityAction takeIncomeAction)
        {
            _takeIncomeAction = takeIncomeAction;
            Hide();
            UpdateInfo(income);
        }


        public async void UpdateInfo(double income, bool withDelay = false)
        {
            _income = income;
            if (withDelay == true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }

            if (_incomeTransform.gameObject.activeSelf == false)
            {
                _incomeText.text = TextMeshProUtils.ConvertBigDoubleToText(_income, 0);
            }
            
            _incomeTransform.gameObject.SetActive(_income > 0);
            _incomeTransform.DOLocalMoveY(_incomeTransform.localPosition.y+1.2f, 0.2f);
            await _incomeTransform.DOScale(1.2f, 0.2f).AsyncWaitForCompletion();
            _incomeText.text = TextMeshProUtils.ConvertBigDoubleToText(_income, 0);
            _incomeTransform.DOScale(1f, 0.2f);
            _incomeTransform.DOLocalMoveY(_incomeTransform.localPosition.y-1.2f, 0.2f);
        }

        protected UniTask AnimationIn()
        {
            return UniTask.CompletedTask;
        }

        protected UniTask AnimationOut()
        {
            return UniTask.CompletedTask;
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

        public void OnPointerDown(PointerEventData eventData)
        {
            _textPoolManager.GetText(TextMeshProUtils.ConvertBigDoubleToText(_income, 0),transform.position);
            
            _takeIncomeAction();
            eventData.Use();
            var pos = Camera.main.WorldToScreenPoint(transform.position);
            _particleManager.PlayUIParticleInPosition(ParticleType.moneySplashSmall, pos,
                Quaternion.identity);
            _particleManager.PlayFollowParticle(CurrencyUIType.Dollar, ParticleType.moneyFollow,
                ParticleType.moneySplashSmall, pos);
        }
    }
}