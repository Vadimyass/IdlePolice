using System;

namespace Gameplay.Scripts.DataProfiling.Models
{
    [Serializable]
    public class BuildingSaveData
    {
        public string Key;
        public bool IsBuilt;
        public int Level;
        public double StoredIncome;
        public string OfficerKey;
    }
}