using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Utils;
using MyBox;
using SolidUtilities.Collections;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using yutokun;

namespace Gameplay.Configs
{
    public class MissionsParser : IParser
    {
        public object ParseObject(string csvString)
        {
            var newPhrases = new Dictionary<int, List<MissionConfigInitializer>>();
            var sheet = CSVParser.LoadFromString(csvString);
            
            for (int i = 0; i < sheet.Count; i++)
            {
                var row = sheet[i];
                
                var phrase = new MissionConfigInitializer();
                phrase.BuildBuildingValue = new KeyValuePair<string, int>();
                phrase.UpgradeBuildingValue = new KeyValuePair<string, int>();
                phrase.ProcessBuildingValue = new KeyValuePair<BuildingProcessName, int>();
                phrase.CurrencyValue = new KeyValuePair<CurrencyUIType, double>();
                
                 if (i < 1) continue;

                phrase.NameKey = row[1];
                phrase.Text = row[3];
                Enum.TryParse(row[13], out SpriteName spriteName);

                phrase.SpriteName = spriteName;
                
                if(phrase.Text == String.Empty) continue;
                
                Enum.TryParse(row[4], out CurrencyUIType currencyName);
                double.TryParse(row[5], out double currencyValue);
                phrase.CurrencyValue = new KeyValuePair<CurrencyUIType, double>(currencyName, currencyValue);
                
                int.TryParse(row[7], out int interactableQuantity);
                phrase.UpgradeBuildingValue = new KeyValuePair<string, int>(row[6], interactableQuantity);

                if (row[8] != string.Empty)
                {
                    phrase.BuildBuildingValue = new KeyValuePair<string, int>(row[8], 1);
                }
                
                Enum.TryParse(row[9], out BuildingProcessName processName);
                int.TryParse(row[10], out int processCount);
                phrase.ProcessBuildingValue = new KeyValuePair<BuildingProcessName, int>(processName, processCount);
                BigDDebugger.LogError(phrase.ProcessBuildingValue.Key);
                
                
                phrase.TargetUpgradeName = row[11];
                
                int.TryParse(row[12], out int targetDistrict);
                phrase.NumberOfDistrictOpen = targetDistrict;
                
                int.TryParse(row[2], out int baseID);

                baseID--;
                
                List<MissionConfigInitializer> list;
                if (newPhrases.ContainsKey(baseID) == false)
                {
                    list = new List<MissionConfigInitializer>();
                    newPhrases.Add(baseID, list);
                }

                newPhrases.TryGetValue(baseID, out list);
                list.Add(phrase);
            }
            
            return newPhrases;
        }
    }
}