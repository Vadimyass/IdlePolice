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
    public class DotsUnlockConfig : IConfig
    {
        private List<DotsUnlockConfigInitializer> _items = new ();
        
        public void LoadConfig(string sheetName)
        {
            var index = _items.Count;
            _items = DataController.ReadSheetFromJson<DotsUnlockConfigInitializer>(sheetName);
        }
        
        
        public DotsUnlockConfigInitializer GetItemByKey(int level, int district)
        {
            var item = _items.FirstOrDefault(x => x.District == district+1 && x.Level == level+1);
            return item;
        }
    }
}