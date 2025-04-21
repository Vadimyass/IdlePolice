using System;
using Agents;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Configs;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Utils;
using UnityEngine;

namespace Gameplay.OrderManaging
{
    public class BetweenBuildingOrder : Order
    {
        private BetweenBuildingsRequest _betweenBuildingsRequest;
        private CarAgent _carAgent;
        private int _currentCount;
        private bool _isAroundBuilding;

        public override void Activate(OrderRequest orderRequest, GroundType groundType)
        {
            _betweenBuildingsRequest = (BetweenBuildingsRequest) orderRequest;
            base.Activate(orderRequest, groundType);
        }

        public override async void PreInteraction(CarAgent carAgent)
        {
            _carAgent = carAgent;
            base.PreInteraction(carAgent);
            _carAgent.Warp(_betweenBuildingsRequest.PreviousBuilding.CarSlotTransform.position);
            _isAroundBuilding = true;
            MoveToNextStep();
            
            Debug.LogError("preinteraction");
        }


        public override void StartInteraction()
        {
            base.StartInteraction();
            MoveToNextStep();
        }

        public override async void Process()
        {
            base.Process();
            Debug.LogError($"process before {_isAroundBuilding}");
            if(_isAroundBuilding == false) return;
            _currentCount++;
            if (_currentCount < _betweenBuildingsRequest.Capacity)
            {
                _carAgent.transform.DOPunchScale(new Vector3(1.2f, 1.6f, 1.2f), 0.6f);
                BigDDebugger.LogError($"current count {_currentCount}");
                return;
            }
            MoveToNextStep();
        }

        public override async void OnEndInteraction()
        {
            base.OnEndInteraction();
            _isAroundBuilding = false;
            
            await _carAgent.SetDestination(_betweenBuildingsRequest.NextBuilding.CarSlotTransform.position);
            Debug.LogError($"OnEndInteraction {_isAroundBuilding}");
            var count = _currentCount;
            for (int i = 0; i < count; i++)
            {
                await _carAgent.transform.DOPunchScale(new Vector3(0.5f, 1.1f, 0.5f), 0.6f).AsyncWaitForCompletion();
                _betweenBuildingsRequest.NextBuilding.AddIteration(1, _carAgent);
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            }

            _currentCount = 0;
            MoveToNextStep();
            PreInteraction(_carAgent);
        }
    }
}