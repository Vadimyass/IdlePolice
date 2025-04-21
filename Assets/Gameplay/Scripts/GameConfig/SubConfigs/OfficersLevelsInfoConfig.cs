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
    public class OfficersLevelsInfoConfig : IConfig
    {
        public  List<OfficerLevelInfoConfigInitializator> Items => _items;
        private List<OfficerLevelInfoConfigInitializator>_items = new ();
        
        public void LoadConfig(string sheetName)
        {
            var index = _items.Count;
            _items = DataController.ReadSheetFromJson<OfficerLevelInfoConfigInitializator>(sheetName);
        }
        
        
        public OfficerLevelInfoConfigInitializator GetItemByKey(OfficerRarity rarity, int level)
        {
            var item = _items.FirstOrDefault(x => x.OfficerRarity == rarity && x.Level == level);
            return item;
        }
        
    }
}