using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Utils;
using SolidUtilities.Collections;
using UnityEngine;

namespace Gameplay.Configs
{
    public class MissionsConfig : IConfig
    {
        private Dictionary<int, List<MissionConfigInitializer>> _items;
        public void LoadConfig(string sheetName)
        {
            _items = DataController.ReadSheetFromJsonToDictionary<MissionConfigInitializer>(sheetName);
        }

        public List<MissionConfigInitializer> GetMissionsForBase(int baseId)
        {
            if (_items.ContainsKey(baseId) == false) return new List<MissionConfigInitializer>();
            
            return _items[baseId];
        }

        public MissionConfigInitializer GetItemByKey(int baseId, string key)
        {
            _items.TryGetValue(baseId, out var list);
            return list.First(x => x.NameKey == key);
        }

        public MissionConfigInitializer GetNextMission(int baseId, string key)
        {
            _items.TryGetValue(baseId, out var list);
            MissionConfigInitializer item;
            if (key == String.Empty)
            {
                item = list.First();
                return item;
            }
            else
            {
                item = list.First(x => x.NameKey == key); 
            }
            
            var index = list.IndexOf(item) + 1;
            if (index >= list.Count) return null;
            return list[index];
        }

    }
}