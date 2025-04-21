using System;
using Agents;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Buildings;
using TMPro;
using UnityEngine;

namespace Gameplay.Scripts.Agents
{
    public class BusAgent : CarAgent
    {
        [SerializeField] private TextMeshProUGUI _textCountGuys;
        private BuildingBusManager _buildingBusManager;
        public int Capacity { get; private set; }
        public int CurrentProgress { get; private set; }
        

        public void Init(int capacity,BuildingBusManager buildingBusManager,Building building)
        {
            _buildingBusManager = buildingBusManager;
            Capacity = capacity;
            LinkedBuilding = building;
            _textCountGuys.text = 0 + "/" + Capacity;
        }

        public void AddProgress()
        {
            CurrentProgress++;
            _textCountGuys.text = CurrentProgress + "/" + Capacity;
            TryActivateChain();
        }

        public override void UpdateSpeed()
        {
            speed = (float) (BaseSpeed * LinkedBuilding.BuildingModifiers.AllBussesSpeedMultiplier);
        }

        public void SetCapacity(int capacity)
        {
            Capacity = capacity;
            _textCountGuys.text = CurrentProgress + "/" + Capacity;
        }

        public void TakeProgress()
        {
            CurrentProgress--;
            _textCountGuys.text = CurrentProgress + "/" + Capacity;
            _buildingBusManager.NextBuilding.AddIteration(1, this);
            //add animation
            Debug.LogError($"take progress {CurrentProgress}");
        }


        public async void TryActivateChain()
        {
            if (_buildingBusManager.NextBuilding == null || _buildingBusManager.NextBuilding.IsBuilt == false)
            {
                gameObject.SetActive(false);
                return;
            }
            
            gameObject.SetActive(true);
            if (CurrentProgress >= Capacity)
            {
                Debug.LogError("activate chain");
                _buildingBusManager.DequeueBus();
                await SetDestination(_buildingBusManager.NextBuilding.CarSlotTransform.position);
                while (CurrentProgress > 0)
                {
                    TakeProgress();
                    await UniTask.Delay(TimeSpan.FromSeconds(1));
                }

                await SetDestination(_buildingBusManager.Building.BusSlotTransform.position);
                _buildingBusManager.EnqueueBus(this);
            }
        }
    }
}