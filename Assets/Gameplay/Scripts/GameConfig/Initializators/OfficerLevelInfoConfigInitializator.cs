using System;
using System.Collections.Generic;
using SolidUtilities.Collections;
using UI;
using UnityEngine;

namespace Gameplay.Configs
{
    [Serializable]
    public class OfficerLevelInfoConfigInitializator : IPhrase
    {
        public OfficerRarity OfficerRarity;
        public int Level;
        public double DonutCost;
        public int UpgradeCardCost;
        public double IncomeMultiplier;
        public double FightPower;
        public double HP;
    }
}