using Agents;
using Cysharp.Threading.Tasks;
using Gameplay.OrderManaging;
using Gameplay.Scripts.Agents;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace Gameplay.Agents
{
    public class AgentsFactory
    {
        private AgentsConfig _agentsConfig;
        private DiContainer _container;

        public AgentsFactory(AgentsConfig agentsConfig,DiContainer container)
        {
            _container = container;
            _agentsConfig = agentsConfig;
        }
        public async UniTask<CarAgent> GetAgent(Building building)
        {
            var agent = _container.InstantiatePrefab(_agentsConfig.GetAgentByType(building.CarAgentType)).GetComponent<CarAgent>();
            var orderRunner = GetRunnerByType(building.CarAgentType);
            _container.Inject(orderRunner);
            await agent.Init(orderRunner,building);
            return agent;
        }
        
        public CarAgentBox GetAgentBox()
        {
            var box = _container.InstantiatePrefab(_agentsConfig.BoxPrefab).GetComponent<CarAgentBox>();
            return box;
        }

        private IOrderRunner GetRunnerByType(AgentType agentType)
        {
            switch (agentType)
            {
                case AgentType.Patrol:
                    return new PatrolOrderRunner();
                case AgentType.Bus:
                    return new BetweenBuildingOrderRunner();
                case AgentType.BoatPatrol:
                    return new PatrolOrderRunner();
                case AgentType.BoatBus:
                    return new BetweenBuildingOrderRunner();
                default:
                    BigDDebugger.LogError($"Add specification of type {agentType} to agentFactory");
                    return null;
            }
        }
    }
}