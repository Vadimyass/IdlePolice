using System.Collections.Generic;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling;
using UI;
using UnityEngine;

namespace Gameplay.Configs
{
    public class MissionConfigInitializer : IPhrase
    {
        public KeyValuePair<CurrencyUIType, double> CurrencyValue;
        public string NameKey;
        public string Text;
        public KeyValuePair<string, int> BuildBuildingValue;
        public KeyValuePair<string, int> UpgradeBuildingValue;
        public string TargetUpgradeName;
        public int NumberOfDistrictOpen;
        public KeyValuePair<BuildingProcessName, int> ProcessBuildingValue;
        public SpriteName SpriteName;
    }
}