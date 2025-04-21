using System.Collections.Generic;
using Gameplay.Scripts.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Scripts.LevelManagement
{
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private List<AssetReference> _levels;

        public AssetReference GetLevelByIndex(int index)
        {
            if (index < _levels.Count)
            {
                return _levels[index];
            }
            
            BigDDebugger.LogError($"Please add new level to config!! with index {index}");
            return null;
        }
    }
}