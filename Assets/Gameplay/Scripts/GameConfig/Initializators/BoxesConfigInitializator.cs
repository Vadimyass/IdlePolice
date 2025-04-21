using System;
using System.Collections.Generic;
using SolidUtilities.Collections;
using UI;
using UnityEngine;

namespace Gameplay.Configs
{
    [Serializable]
    public class BoxesConfigInitializator : IPhrase
    {
        public GachaBoxType Key;
        public double RareChance;
        public double EpicChance;
        public double LegendaryChance;
        public int GaranteeQuantity;
        public OfficerRarity Garantee;
    }
    public enum GachaBoxType
    {
        Basic,
        Advanced,
        Expert,
    }
}