using System.Collections.Generic;
using Gameplay.OrderManaging;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling;
using UI;
using UnityEngine;

namespace Gameplay.Configs
{
    public class DotsUnlockConfigInitializer : IPhrase
    {
        public int Level;
        public int District;
        public PointType PointType;
    }
}