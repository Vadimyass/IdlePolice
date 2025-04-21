using System;
using System.Collections.Generic;
using System.Linq;
using Agents;
using BigD.Config;
using Gameplay.Configs;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Locker;
using Gameplay.Scripts.TimeManagement;
using Gameplay.Scripts.Utils;
using MyBox;
using UnityEngine;
using Zenject;

namespace Gameplay.OrderManaging
{
    public class OrderManager
    {
        private List<IOrder> _activeOrders = new ();
        private Queue<PatrolOrderRunner> _orderCarSeekers = new ();
        private Queue<PatrolOrderRunner> _orderBoatSeekers = new ();
        private SignalBus _signalBus;
        private double _orderInterval = 2;
        private int _timerTicks;
        private LevelController _levelController;
        private GameConfig _gameConfig;
        private DiContainer _container;
        private AgentsManager _agentsManager;
        private BaseUpgradesController _baseUpgradesController;

        private double _crimesCount = 1;
        private LockController _lockController;

        [Inject]
        private void Construct(SignalBus signalBus, LockController lockController, LevelController levelController,GameConfig gameConfig,DiContainer container,AgentsManager agentsManager,BaseUpgradesController baseUpgradesController)
        {
            _lockController = lockController;
            _baseUpgradesController = baseUpgradesController;
            _agentsManager = agentsManager;
            _container = container;
            _gameConfig = gameConfig;
            _levelController = levelController;
            _signalBus = signalBus;
        }

        public void SetCrimesCount(double crimesCount)
        {
            _crimesCount = crimesCount;
        }

        public void SetOrderInterval(double interval)
        {
            _orderInterval = interval;
        }

        public void Init()
        {
            _signalBus.Subscribe<TimeTickSignal>(OnTimeTick);
            _signalBus.Subscribe<BaseUpgradeSignal>(OnBaseUpgrade);
            ForceUpdateModificators();
            TryActivateOrder();
        }

        private void OnBaseUpgrade(BaseUpgradeSignal obj)
        {
            if (obj.BuildingName == BuildingName.All)
            {
                ForceUpdateModificators();
            }
        }

        private void ForceUpdateModificators()
        {
            _crimesCount = _baseUpgradesController.GetInfoForBuilding(BuildingName.All).Crimes;
            _orderInterval = _baseUpgradesController.GetInfoForBuilding(BuildingName.All).CrimeInterval;
            
            //BigDDebugger.LogError($"Crimes count: {_crimesCount}", $"Order interval: {_orderInterval}");
        }

        public void AddSeeker(PatrolOrderRunner patrolOrderRunner, GroundType groundType)
        {
            switch (groundType)
            {
                case GroundType.Road:
                    _orderCarSeekers.Enqueue(patrolOrderRunner);
                    break;
                case GroundType.Water:
                    _orderBoatSeekers.Enqueue(patrolOrderRunner);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(groundType), groundType, null);
            }
            
            if (_activeOrders.Count > 0)
            {
                TryAssignOrder();
            }
        }

        private void OnTimeTick()
        {
            if (_timerTicks >= _orderInterval)
            {
                _timerTicks = 0;
                TryActivateOrder();
                return;
            }

            _timerTicks++;
        }
        
        public void TryActivateOrder()
        {
            //BigDDebugger.LogError($"Active order counts: {_activeOrders.Count}", $"Crimes count now: {_crimesCount}");
            if(_lockController.HaveLock<OrderSpawnLocker>()) return;
            
            if(_activeOrders.Count >= _crimesCount) return;
            
            var order = GetRandomAvailableOrder();
            var point = _levelController.CurrentLevel.PointsContainer.GetRandomPoint(_levelController.CurrentLevel
                .GetRandomIndexOfOpenedAreas(), _levelController.CurrentLevel.AvailableGroundTypes.GetRandom());
            if (point == null)
            {
                return;
            }
            
            order.Activate(new DefaultOrderRequest()
            {
                LevelController =  _levelController,
                OrderPoint =  point,
                OrderManager = this,
                GameConfig = _gameConfig,
            }, point.GroundType);
            _container.Inject(order);
            _activeOrders.Add(order);
            TryAssignOrder();
        }

        public void DeactivateOrder(IOrder order)
        {
            if(_activeOrders.Contains(order) == false) return;
            
            _activeOrders.Remove(order);

        }

        public IOrder GetRandomAvailableOrder()
        {
            var availableOrder = _levelController.CurrentLevel.AvailableOrderTypes.GetRandom();

            return (IOrder)Activator.CreateInstance(availableOrder);
        }

        public void TryAssignOrder()
        {
            var seekers = _orderCarSeekers.ToList();
            seekers.AddRange(_orderBoatSeekers.ToList());

            for (int i = 0; i < seekers.Count; i++)
            {
                var dequeuedCar = _orderCarSeekers.TryPeek(out var carSeeker);

                var dequeuedBoat = _orderBoatSeekers.TryPeek(out var boatSeeker);

                IOrder order = default;
                PatrolOrderRunner orderSeeker = null;

                if (dequeuedCar && _activeOrders.Any(x =>
                        x.OrderBlockType == OrderBlockType.Activated && x.GroundType == GroundType.Road))
                {
                    order = _activeOrders.First(x =>
                        x.OrderBlockType == OrderBlockType.Activated && x.GroundType == GroundType.Road);
                    _orderCarSeekers.TryDequeue(out orderSeeker);

                }
                else if (dequeuedBoat && _activeOrders.Any(x =>
                             x.OrderBlockType == OrderBlockType.Activated && x.GroundType == GroundType.Water))
                {
                    order = _activeOrders.First(x =>
                        x.OrderBlockType == OrderBlockType.Activated && x.GroundType == GroundType.Water);
                    _orderBoatSeekers.TryDequeue(out orderSeeker);
                }
                else
                {
                    return;
                }

                _activeOrders.Remove(order);
                orderSeeker.ActivateOrder(order);

            }

        }

    }
}