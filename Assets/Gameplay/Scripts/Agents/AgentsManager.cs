using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.Agents;
using Gameplay.Scripts.Agents;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.LevelManagement;
using UnityEngine;
using Zenject;

namespace Agents
{
    public class AgentsManager
    {
        private AgentsFactory _agentsFactory;
        private DiContainer _diContainer;
        private LevelController _levelController;

        private List<CarAgent> _carAgents = new();
        public int GetCarCount => _carAgents.Count(x => x.AgentType == AgentType.Patrol);

        public AgentsManager(AgentsConfig agentsConfig,DiContainer diContainer,LevelController levelController)
        {
            _levelController = levelController;
            _agentsFactory = new AgentsFactory(agentsConfig,diContainer);
        }
        
        public void Init()
        {
        }

        public CarAgentBox CreateCarBox(Building building)
        {
            var box = _agentsFactory.GetAgentBox();
            box.transform.position = building.CarSlotTransform.position;
            return box;
        }
        
        public async UniTask<CarAgent> AddFreeAgent(Building building)
        {
            var carAgent = await _agentsFactory.GetAgent(building);
            switch (building.CarAgentType)
            {
                case AgentType.Default:
                    Debug.LogError("Unexpected default value");
                    break;
                case AgentType.Patrol:
                    carAgent.Warp(building.CarSlotTransform.position);
                    break;
                case AgentType.Bus:
                    carAgent.Warp(building.BusSlotTransform.position);
                    break;
                case AgentType.BoatPatrol:
                    carAgent.Warp(building.CarSlotTransform.position);
                    break;
                case AgentType.BoatBus:
                    carAgent.Warp(building.BusSlotTransform.position);
                    break;
            }
            _carAgents.Add(carAgent);
            return carAgent;
        }
        
    }
}