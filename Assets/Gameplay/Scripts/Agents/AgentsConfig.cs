using Agents;
using Gameplay.Scripts.Agents;
using Gameplay.Scripts.Utils;
using RotaryHeart.Lib.SerializableDictionary;
using SolidUtilities.Collections;
using UnityEngine;

namespace Gameplay.Agents
{
    public class AgentsConfig : ScriptableObject
    {
        [SerializeField] private SerializableDictionaryBase<AgentType, CarAgent> _agents;
        [SerializeField] private CarAgentBox _boxPrefab;

        public CarAgentBox BoxPrefab => _boxPrefab;
        
        public CarAgent GetAgentByType(AgentType agentType)
        {
            if (_agents.TryGetValue(agentType, out var agent))
            {
                return agent;
            }
            else
            {
                BigDDebugger.LogError($"There`s error! add following type {agentType} to AgentsConfig");
                return null;
            }
        }
        
    }
}