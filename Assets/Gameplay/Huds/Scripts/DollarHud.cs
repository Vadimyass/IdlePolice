using System;
using Agents;
using Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.OrderManaging;
using Gameplay.Scripts._3DTextPoolManagement;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Particles;
using TMPro;
using UI.UIUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Gameplay.Huds.Scripts
{
    public class DollarHud : MonoBehaviour , IPointerClickHandler
    {
        private Action _onComplete;
        private CarAgent _carAgent;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private OrderPoint _orderPoint;
        private ParticleManager _particleManager;
        private TextPoolManager _textPoolManager;
        private AudioManager _audioManager;

        [Inject]
        private void Construct(PlayerPrefsSaveManager playerPrefsSaveManager, AudioManager audioManager, ParticleManager particleManager,TextPoolManager textPoolManager)
        {
            _audioManager = audioManager;
            _textPoolManager = textPoolManager;
            _particleManager = particleManager;
            _playerPrefsSaveManager = playerPrefsSaveManager;
        }
        
        public void Init(Action onComplete)
        {
            _onComplete = onComplete;
        }

        public void SetCarAgent(CarAgent carAgent)
        {
            _carAgent = carAgent;
        }

        public void SetOrderPoint(OrderPoint orderPoint)
        {
            _orderPoint = orderPoint;
        }

        public void SetActive(bool isShow)
        {
            gameObject.SetActive(isShow);
            if (isShow)
            {
                Activate();
            }
        }


        public async void TakeIncome()
        {
            _textPoolManager.GetText(TextMeshProUtils.ConvertBigDoubleToText(_carAgent.LinkedBuilding.RealIncome),transform.position);
            
            _onComplete();
            _orderPoint.SetOccupied(false);
            
            _audioManager.PlaySound(TrackName.TakeMoney);

            var pos = Camera.main.WorldToScreenPoint(transform.position);
            _particleManager.PlayUIParticleInPosition(ParticleType.moneySplashSmall, pos,
                Quaternion.identity);
            await _particleManager.PlayFollowParticle(CurrencyUIType.Dollar, ParticleType.moneyFollow,
                ParticleType.moneySplashSmall, pos);
            _playerPrefsSaveManager.PrefsData.CurrenciesModel.ChangeMoney(_carAgent.LinkedBuilding.RealIncome,0);
        }

        public void TakeIncomeWithoutAnimation()
        {
            _onComplete();
            _orderPoint.SetOccupied(false);
        }
        
        public void Deactivate()
        {

        }

        private void Activate()
        {
            transform.localScale = Vector3.zero;
            
            StartAnimation();
        }
        
        private async void StartAnimation()
        {
            var startPos = new Vector3(0, 0f, -1.55f);
            var vector = transform.localPosition;
            var seq = DOTween.Sequence();
            seq.Append(transform.DOScale(Vector3.one, 0.5f));
            seq.Join(transform.DOLocalMove(vector, 0.5f).From(startPos));
            await seq.AsyncWaitForCompletion();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            TakeIncome();
        }
    }
}