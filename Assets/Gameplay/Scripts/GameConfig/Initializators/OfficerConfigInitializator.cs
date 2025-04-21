using System;
using System.Collections.Generic;
using SolidUtilities.Collections;
using UI;
using UnityEngine;

namespace Gameplay.Configs
{
    [Serializable]
    public class OfficerConfigInitializator : IPhrase
    {
        public string Key;
        public OfficerType OfficerType;
        public OfficerRarity OfficerRarity;
        public SpriteName SpriteName;
        public string Description;
    }

    public enum OfficerRarity
    {
        Rare,
        Epic,
        Legendary,
        Mythical
    }
    
    public enum OfficerType
    {
        STR,
        DTC,
        CAR
    }
}