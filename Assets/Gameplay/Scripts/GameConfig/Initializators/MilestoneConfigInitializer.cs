using System;
using System.Collections.Generic;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling;
using UI;
using UnityEngine;

namespace Gameplay.Configs
{
    public class MilestoneConfigInitializer : IPhrase
    {
        public KeyValuePair<MilestoneRewardType, double> MilestoneReward;
        public int Number;
        public int MissionsTarget;
    }

    public enum MilestoneRewardType
    {
        Crystal,
        Basic_box,
        Advanced_box
    }
}