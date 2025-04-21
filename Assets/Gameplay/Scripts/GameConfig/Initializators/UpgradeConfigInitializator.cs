using System;
using System.Collections.Generic;
using Gameplay.OrderManaging;
using SolidUtilities.Collections;
using UI;
using UnityEngine;

namespace Gameplay.Configs
{
    [Serializable]
    public class UpgradeConfigInitializator : IPhrase
    {
        public int Level;
        public double UpgradeCost;
        public double Income;
        public float Duration;
        public int Cars;
        public int Multiplier;
        public SpriteName SpriteName;
        public string UpgradeName;
        public AdditiveParamName AdditiveParamName;
        public PointType AdditiveParamValue;
    }

    public enum AdditiveParamName
    {
        Open_dots
    }
}