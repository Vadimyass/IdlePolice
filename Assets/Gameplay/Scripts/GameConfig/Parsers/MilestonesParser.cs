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
    public class MilestonesParser : IParser
    {
        public object ParseObject(string csvString)
        {
            var newPhrases = new Dictionary<int, List<MilestoneConfigInitializer>>();
            var sheet = CSVParser.LoadFromString(csvString);
            
            for (int i = 0; i < sheet.Count; i++)
            {
                var row = sheet[i];
                
                var phrase = new MilestoneConfigInitializer();
                phrase.MilestoneReward = new KeyValuePair<MilestoneRewardType, double>();

                if (i < 1) continue;

                int.TryParse(row[0], out int number);
                phrase.Number = number;
                
                int.TryParse(row[2], out int targetMission);
                Debug.LogError(targetMission);
                phrase.MissionsTarget = targetMission;
                
                Enum.TryParse(row[3], out MilestoneRewardType currencyName);
                double.TryParse(row[4], out double currencyValue);
                phrase.MilestoneReward = new KeyValuePair<MilestoneRewardType, double>(currencyName, currencyValue);
                
                int.TryParse(row[1], out int baseID);

                baseID--;
                
                List<MilestoneConfigInitializer> list;
                if (newPhrases.ContainsKey(baseID) == false)
                {
                    list = new List<MilestoneConfigInitializer>();
                    newPhrases.Add(baseID, list);
                }

                newPhrases.TryGetValue(baseID, out list);
                list.Add(phrase);
            }
            
            return newPhrases;
        }
    }
}