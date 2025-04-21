using System.Collections.Generic;
using System.Linq;
using Agents;
using BigD.Config;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts.Agents;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.Buildings
{
    public class BuildingBusManager : MonoBehaviour
    {
        private Building _building;
        private Building _nextBuilding;

        public Building Building => _building;
        public Building NextBuilding => _nextBuilding;
        
        private AgentsManager _agentsManager;

        private Queue<BusAgent> _agents = new ();
        private List<BusAgent> _createdAgents = new();
        private int _currentBusCapacity;
        private GameConfig _gameConfig;

        [Inject]
        private void Construct(AgentsManager agentsManager, GameConfig gameConfig, LevelController levelController)
        {
            _gameConfig = gameConfig;
            _agentsManager = agentsManager;
        }
        
        public async UniTask Init(Building building,Building nextBuilding)
        {
            if(building.BuildingName == BuildingName.All || building.BuildingName == BuildingName.Dispatch_center) return;
            
            _building = building;
            _nextBuilding = nextBuilding;
            
            var capacity = _gameConfig.EconomyConfig.GetItemByKey(EconomyEnum.Bus_capacity_start);
            await CreateCar();
            TrySetCapacity((int)capacity);
            
            if (_nextBuilding != null)
            {
                ShowBus(_nextBuilding.IsBuilt);
            }
            else
            {
                ShowBus(false);
            }
        }

        public bool TryAddIteration()
        {
            if (_agents.TryPeek(out var bus))
            {
                bus.AddProgress();
                return true;
            }

            return false;
        }

        public bool IsBusFull()
        {
            _agents.TryPeek(out var busAgent);
            return busAgent.CurrentProgress == busAgent.Capacity;
        }
        
        
        public void TryValidateSpeed()
        {
            foreach (var agent in _createdAgents)
            {
                agent.UpdateSpeed();
            }
        }

        public void TrySetCapacity(int capacity)
        {
            foreach (var agent in _agents)
            {
                _currentBusCapacity = capacity;
                agent.SetCapacity(_currentBusCapacity);
            }
        }
        

        private async UniTask CreateCar()
        {
            var busAgent = (BusAgent) await _agentsManager.AddFreeAgent(_building);
            busAgent.Init(_currentBusCapacity,this,_building);
            _createdAgents.Add(busAgent);
            _agents.Enqueue(busAgent);
        }


        public void DequeueBus()
        {
            _agents.Dequeue();
        }

        public void EnqueueBus(BusAgent busAgent)
        {
            _agents.Enqueue(busAgent);
        }

        public void ShowBus(bool isShow)
        {
            GetFirstBusAgent().gameObject.SetActive(isShow);
        }
        
        public BusAgent GetFirstBusAgent()
        {
            var bus = _createdAgents.FirstOrDefault();
            if (bus == null)
            {
                CreateCar();
            }

            return _createdAgents.FirstOrDefault();
        }
    }


}