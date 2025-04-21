using System;
using System.Collections.Generic;
using SolidUtilities.Collections;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Configs
{
    [Serializable]
    public class BuildingConfigInitializator : IPhrase
    {
        public string Key;
        public int AutomateLvl;
        public double Cost;
        public OfficerType BuildingType; 
        public SpriteName SpriteName;
        public SpriteName BuildPhoto;
        public string Description;
        public string Name;
        public List<UpgradeConfigInitializator> Upgrades;
    }
    
}