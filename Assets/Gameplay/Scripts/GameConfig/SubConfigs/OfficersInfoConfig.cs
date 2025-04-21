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
    public class OfficersInfoConfig : IConfig
    {
        public List<OfficerConfigInitializator> Items => _items;
        private List<OfficerConfigInitializator>_items = new ();
        
        public void LoadConfig(string sheetName)
        {
            var index = _items.Count;
            _items = DataController.ReadSheetFromJson<OfficerConfigInitializator>(sheetName);
        }
        
        
        public OfficerConfigInitializator GetItemByKey(string key)
        {
            var item = _items.FirstOrDefault(x => x.Key == key);
            return item;
        }
        
    }
}