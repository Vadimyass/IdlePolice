using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SolidUtilities.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.Scripts.DataProfiling.PrefsData
{
    public class BaseUpgradesModel : IPlayerPrefsData
    {
        [JsonIgnore] public SerializableDictionary<int, List<string>> BoughtUpgrades => _boughtUpgrades;
        [JsonProperty] private SerializableDictionary<int, List<string>> _boughtUpgrades = new SerializableDictionary<int, List<string>>();

        public void Initialize()
        {
            
        }

        public bool CheckIsBought(string key, int baseId)
        {
            _boughtUpgrades.TryGetValue(baseId, out var value);
            if (value == null) return false;
            
            return value.Any(x => x == key);
        }

        public void AddAsBought(string key, int baseId)
        {
            Debug.LogError(key);
            if (_boughtUpgrades.ContainsKey(baseId) == false)
            {        Debug.LogError(key);
                var upgrades = new List<string> { key };
                _boughtUpgrades.Add(baseId, upgrades);
            }

            if(_boughtUpgrades[baseId].Contains(key) == true) return;
            Debug.LogError(key);
            _boughtUpgrades[baseId].Add(key);
        }
    }
}