using System.Collections.Generic;
using System.Linq;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Utils;
using ModestTree;
using SolidUtilities.Collections;
using UnityEngine;
using Zenject;

namespace Gameplay.Configs
{
    public class UpgradeConfig : IConfig
    {
        private SerializableDictionary<int,List<BuildingConfigInitializator>> _items = new ();
        
        public void LoadConfig(string sheetName)
        {
            var index = _items.Count;
            _items.Add(index,DataController.ReadSheetFromJson<BuildingConfigInitializator>(sheetName));
        }

        public BuildingConfigInitializator GetBuildingByKey(string key, int baseId = -1)
        {
            var item = _items[baseId].FirstOrDefault(x => x.Key == key);
            return item;
        }
        
        public UpgradeConfigInitializator GetItemByKey(string key, int level, int baseId = -1)
        {
            var item = _items[baseId].FirstOrDefault(x => x.Key == key);
            if (item.Upgrades.Count < level)
            {
                return null;
            }
            return item.Upgrades.First(x => x.Level == level);
        }

        public UpgradeConfigInitializator GetNextStageByLevel(string key, int level, int baseId = -1)
        {
            var item = _items[baseId].FirstOrDefault(x => x.Key == key);
            if (item.Upgrades.Count <= level)
            {
                return null;
            }
            return item.Upgrades.First(x => x.Multiplier != 0 && x.Level > level);
        }
        
        public UpgradeConfigInitializator GetPreviousStageByLevel(string key, int level, int baseId = -1)
        {
            var item = _items[baseId].FirstOrDefault(x => x.Key == key);
            return item.Upgrades.Last(x => x.Multiplier != 0 && x.Level <= level);
        }
    }
}