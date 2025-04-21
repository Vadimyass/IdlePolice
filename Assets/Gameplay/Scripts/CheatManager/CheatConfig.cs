using System;
using System.Collections.Generic;
using System.Linq;
using TypeReferences;
using UnityEngine;

namespace Gameplay.Scripts.CheatManager
{
    public class CheatConfig : ScriptableObject
    {
        [SerializeField] private List<CheatData> cheatData = default;

        public List<Type> GetCheatTypes()
        {
            return cheatData.Select(data => data.CheatItemType.Type).ToList();
        }
    }
    
    [System.Serializable]
    public class CheatData
    {
        [Inherits(typeof(ICheat))]
        [SerializeField] private TypeReference cheatItemType;

        public TypeReference CheatItemType => cheatItemType;
    }
}