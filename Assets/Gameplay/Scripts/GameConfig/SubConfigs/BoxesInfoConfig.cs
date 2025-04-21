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
    public class BoxesInfoConfig : IConfig
    {
        public List<BoxesConfigInitializator> Items => _items;
        private List<BoxesConfigInitializator>_items = new ();
        
        public void LoadConfig(string sheetName)
        {
            var index = _items.Count;
            _items = DataController.ReadSheetFromJson<BoxesConfigInitializator>(sheetName);
        }
        
        
        public BoxesConfigInitializator GetItemByKey(GachaBoxType key)
        {
            var item = _items.FirstOrDefault(x => x.Key == key);
            return item;
        }
        
    }
}