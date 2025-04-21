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
    public class UpgradeBaseConfig : IConfig
    {
        public SerializableDictionary<int,List<BaseUpgradeConfigInitializator>> Items => _items;
        private SerializableDictionary<int,List<BaseUpgradeConfigInitializator>> _items = new ();
        
        public void LoadConfig(string sheetName)
        {
            var index = _items.Count;
            _items.Add(index,DataController.ReadSheetFromJson<BaseUpgradeConfigInitializator>(sheetName));
        }
        
        
        public BaseUpgradeConfigInitializator GetItemByKey(string key, int baseId = -1)
        {
            var item = _items[baseId].FirstOrDefault(x => x.Key == key);
            return item;
        }
        
    }
}