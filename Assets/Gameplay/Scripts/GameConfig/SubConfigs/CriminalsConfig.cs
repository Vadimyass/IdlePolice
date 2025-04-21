using System.Collections.Generic;
using Gameplay.Scripts.DataProfiling;
using UnityEngine;

namespace Gameplay.Configs
{
    public class CriminalsConfig : IConfig
    {
        private List<CriminalInitializator> _criminalInitializators = new();
        public void LoadConfig(string sheetName)
        {
            _criminalInitializators = DataController.ReadSheetFromJson<CriminalInitializator>(sheetName);
        }

        public CriminalInitializator GetCriminal()
        {
            return _criminalInitializators[0];
        }
    }
}