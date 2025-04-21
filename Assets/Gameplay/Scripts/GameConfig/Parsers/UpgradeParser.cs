using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Gameplay.OrderManaging;
using Gameplay.Scripts.Utils;
using MyBox;
using SolidUtilities.Collections;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using yutokun;

namespace Gameplay.Configs
{
    public class UpgradeParser : IParser
    {
         public object ParseObject(string csvString)
        {
            var newPhrases = new List<BuildingConfigInitializator>();
            var sheet = CSVParser.LoadFromString(csvString);
            for (int i = 0; i < sheet.Count; i++)
            {
                var row = sheet[i];

                if (i < 1) continue;

                if (row[0] == string.Empty)
                {
                    double.TryParse(row[2], out double cost);
                    if (cost > 0)
                    {
                        var building = new BuildingConfigInitializator();
                        building.Key = row[1];
                        
                        Enum.TryParse(row[15],out OfficerType buildingType);
                        building.BuildingType = buildingType;
                        
                        building.Cost = cost;
                        
                        int.TryParse(row[14], out int capoLevel);
                        building.AutomateLvl = capoLevel;

                        building.Name = row[17];
                        
                        building.Upgrades = new List<UpgradeConfigInitializator>();
                        newPhrases.Add(building);
                    }
                }
                else
                {
                    var phrase = new UpgradeConfigInitializator();
                    
                    int.TryParse(row[0], out int level);
                    phrase.Level = level;
                    
                    double.TryParse(row[2], out double cost);
                    phrase.UpgradeCost = cost;

                    double.TryParse(row[3], out  double income);
                    phrase.Income = income;
                
                    int.TryParse(row[4], out int duration);
                    phrase.Duration = duration;
                
                    int.TryParse(row[5], out int cars);
                    phrase.Cars = cars;
    
                    int.TryParse(row[7], out int multiplier);
                    phrase.Multiplier = multiplier;
                    
                    Enum.TryParse(row[16], out SpriteName spriteName);
                    phrase.SpriteName = spriteName;

                    phrase.UpgradeName = row[17];
                    Enum.TryParse(row[18], out AdditiveParamName additiveParam);
                    phrase.AdditiveParamName = additiveParam;
                    
                    Enum.TryParse(row[19], out PointType pointType);
                    phrase.AdditiveParamValue = pointType;
                
                    newPhrases.Last().Upgrades.Add(phrase);
                }
            }
            return newPhrases;
        }
    }
}