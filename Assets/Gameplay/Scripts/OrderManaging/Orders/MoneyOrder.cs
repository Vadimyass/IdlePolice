using System;
using System.Threading;
using Agents;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Huds.Scripts;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Utils;
using Particles;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.OrderManaging
{
    public class MoneyOrder : Order
    {
        private OrderPoint _orderPoint;
        private CarAgent _carAgent;
        private LevelController _levelController;
        private OrderManager _orderManager;
        private OrderHud _orderHud;
        
        private CancellationTokenSource _cancellationTokenSource;
        private SignalBus _signalBus;
        private ParticleManager _particleManager;


        [Inject]
        private void Construct(SignalBus signalBus, ParticleManager particleManager)
        {
            _particleManager = particleManager;
            _signalBus = signalBus;
        }
        
        public override void Activate(OrderRequest orderRequest, GroundType groundType)
        {
            var order = (DefaultOrderRequest)orderRequest;
            _cancellationTokenSource = new CancellationTokenSource();
            _levelController = order.LevelController;
            _orderPoint = order.OrderPoint;
            _orderManager = order.OrderManager;
            _orderPoint.SetOccupied(true);
            _orderHud = _levelController.CurrentLevel.PointsContainer.GetHud(this);
            _orderHud.DollarHud.SetOrderPoint(_orderPoint);
            _orderHud.SetPosition(_orderPoint.transform.position);
            base.Activate(orderRequest, groundType);
            
        }

        public override async void PreInteraction(CarAgent carAgent)
        {
            _carAgent = carAgent;
            _carAgent.StopIdle();
            base.PreInteraction(_carAgent);
            var isCancelled = await UniTask.Delay(TimeSpan.FromSeconds(1)).AttachExternalCancellation(_cancellationTokenSource.Token).SuppressCancellationThrow();
            if (isCancelled)
            {
                _carAgent.StartIdle();
                return;
            }
            _orderHud.DollarHud.SetCarAgent(carAgent);
            isCancelled = await _carAgent.SetDestination(_orderPoint.transform.position)
                .AttachExternalCancellation(_cancellationTokenSource.Token).SuppressCancellationThrow();
            
            if (isCancelled)
            {
                _carAgent.StartIdle();
                return;
            }
            MoveToNextStep();
        }

        public override async void StartInteraction()
        {
            _orderHud.ChangeHud(OrderType.Arrest);
            _orderHud.SetSprite(_carAgent.LinkedBuilding.GetCurrentUpgradeSpriteName());
            _orderHud.ArrestHud.Activate((float) _carAgent.LinkedBuilding.RealDuration,false);
            _carAgent.StartFightAnimation();
            base.StartInteraction();
            MoveToNextStep();
        }

        public override async void Process()
        {
            base.Process();
            await UniTask.Delay(TimeSpan.FromSeconds(_carAgent.LinkedBuilding.RealDuration));
            MoveToNextStep();
        }

        public override async void OnEndInteraction()
        {
            base.OnEndInteraction();
            _signalBus.Fire(new ProcessFinishSignal(BuildingProcessName.Arrest));
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _orderHud.ChangeHud(OrderType.Dollar);
            _carAgent.StopFightAnimation();
            if (_carAgent.LinkedBuilding.IsAutomated)
            {
                MoveMoneyToBuilding();
            }
            Complete();
            MoveToNextStep();
            _carAgent.StartIdle();
        }

        private async void MoveMoneyToBuilding()
        {
            _orderHud.DollarHud.TakeIncomeWithoutAnimation();
            _particleManager.PlayParticleInPosition(ParticleType.money3dSplash, _carAgent.position+Vector3.up,
                Quaternion.identity,1);
            var particle = _particleManager.PlayParticleInPosition(ParticleType.money3dFollow, _carAgent.position+Vector3.up,
                Quaternion.identity, 3);
            await particle.transform.DOMove(_carAgent.LinkedBuilding.transform.position+Vector3.up, 1.5f).SetEase(Ease.Linear).AsyncWaitForCompletion();
            _carAgent.LinkedBuilding.AddMoneyForCarHud(_carAgent.LinkedBuilding.RealIncome);
        }
        
        public override void CompleteOrder()
        {
            base.CompleteOrder();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            _orderPoint.SetOccupied(false);
            if(_carAgent == null) return;
            
            _carAgent.ResetPath();
            _carAgent.OrderRunner.CompleteOrder();
        }

        public void Complete()
        {
            CompleteOrder();
        }
    }
}