using System;
using System.Threading;
using Agents;
using BigD.Config;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Configs;
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
    public class PrisonOrder : Order
    {
        private OrderPoint _orderPoint;
        private CarAgent _carAgent;
        private LevelController _levelController;
        private OrderManager _orderManager;
        private GameConfig _gameConfig;
        private OrderHud _orderHud;
        private CriminalInitializator _criminalInitializator;
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
            _gameConfig = order.GameConfig;
            _orderPoint.SetOccupied(true);
            _criminalInitializator = _gameConfig.CriminalsConfig.GetCriminal();
            _orderHud = _levelController.CurrentLevel.PointsContainer.GetHud(this);
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
            _orderHud.DollarHud.SetOrderPoint(_orderPoint);
            isCancelled = await _carAgent.SetDestination(_orderPoint.transform.position)
                .AttachExternalCancellation(_cancellationTokenSource.Token).SuppressCancellationThrow();
            
            if (isCancelled)
            {
                _carAgent.StartIdle();
                return;
            }
            MoveToNextStep();
        }

        public override void StartInteraction()
        {
            _orderHud.ChangeHud(OrderType.Arrest);
            _orderHud.SetSprite(_carAgent.LinkedBuilding.GetCurrentUpgradeSpriteName());
            _orderHud.ArrestHud.Activate((float) _carAgent.LinkedBuilding.RealDuration);
            _carAgent.StartFightAnimation();
            base.StartInteraction();
            MoveToNextStep();
        }

        public override async void Process()
        {
            base.Process();
            await UniTask.Delay(TimeSpan.FromSeconds((float) _carAgent.LinkedBuilding.RealDuration + 1));
            MoveToNextStep();
        }

        public override async void OnEndInteraction()
        {
            base.OnEndInteraction();
            _signalBus.Fire(new ProcessFinishSignal(BuildingProcessName.Arrest));
            _orderHud.ChangeHud(OrderType.Dollar);
            _carAgent.StopFightAnimation();
            if (_carAgent.LinkedBuilding.IsAutomated)
            {
                MoveMoneyToBuilding();
            }

            var building = _carAgent.LinkedBuilding.NextBuilding;

            if (building == null)
            {
                BigDDebugger.LogError("NO NEXT BUILDING IN", _carAgent.LinkedBuilding);
                Complete();
                MoveToNextStep();
                _carAgent.StartIdle();
                return;
            }
            
            BigDDebugger.LogError(building.BuildingName, building.IsBuilt);
            if (building.IsBuilt == true)
            {
                _carAgent.ShowCriminalHud();
                await _carAgent.SetDestination(building.CarSlotTransform.position);
                building.AddIteration(1, _carAgent);
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                _carAgent.HideCriminalHud();
            }
            
            Complete();
            MoveToNextStep();
            _carAgent.StartIdle();
            
        }

        private async void MoveMoneyToBuilding()
        {
            _orderPoint.SetOccupied(false);
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
            if(_carAgent is null) return;
            
            _carAgent.ResetPath();
            _carAgent.OrderRunner.CompleteOrder();
        }

        private void Complete()
        {
            CompleteOrder();
        }
    }
}