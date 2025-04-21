using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Utils;
using SolidUtilities.Collections;
using UnityEngine;

namespace Gameplay.Configs
{
    public class MilestonesConfig : IConfig
    {
        private Dictionary<int, List<MilestoneConfigInitializer>> _items;
        public void LoadConfig(string sheetName)
        {
            _items = DataController.ReadSheetFromJsonToDictionary<MilestoneConfigInitializer>(sheetName);
        }

        public List<MilestoneConfigInitializer> GetMilestonesForBase(int baseId)
        {
            if (_items.ContainsKey(baseId) == false) return new List<MilestoneConfigInitializer>();
            
            return _items[baseId];
        }

        public MilestoneConfigInitializer GetItemByKey(int baseId, int number)
        {
            _items.TryGetValue(baseId, out var list);
            return list.First(x => x.Number == number);
        }
        
        
        public MilestoneConfigInitializer GetNextMilestone(int baseId, int number)
        {

            _items.TryGetValue(baseId, out var list);
            MilestoneConfigInitializer item;
            Debug.LogError(number);
            item = list.FirstOrDefault(x => x.Number == number+1);
            if (item == null) return null;
            
            return item;
        }

        public int GetFullCountOfMissionsForMilestone(int baseId, int number)
        {
            _items.TryGetValue(baseId, out var list);
            MilestoneConfigInitializer item;
            var count = 0;

            for (int i = 0; i < number; i++)
            {
                count += list[i].MissionsTarget;
            }
            return count;
        }

    }
}