using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Gameplay.Scripts.Utils;
using MyBox;
using SolidUtilities.Collections;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using yutokun;

namespace Gameplay.Configs
{
    public class UpgradeBaseParser : IParser
    {
        public object ParseObject(string csvString)
        {
            var newPhrases = new List<BaseUpgradeConfigInitializator>();
            var sheet = CSVParser.LoadFromString(csvString);
            for (int i = 0; i < sheet.Count; i++)
            {
                var phrase = new BaseUpgradeConfigInitializator();
                var row = sheet[i];

                if (i < 1) continue;
                
                if (row[1] == string.Empty) break;

                phrase.AdditiveValue = new SerializableDictionary<AdditiveType, double>();
                for (int j = 5; j < 11; j+=2)
                {
                    Enum.TryParse(row[j],out AdditiveType currencyType);
                    var paramValue = row[j+1].Replace("'", "");
                    double.TryParse(paramValue, NumberStyles.Any,CultureInfo.InvariantCulture,out double value);
                    if (value != 0)
                    {
                        try
                        {
                            phrase.AdditiveValue.Add(currencyType,value);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(paramValue);
                            Debug.Log(value);
                            Debug.LogError(row[0]);
                            Debug.LogError(row[j]);
                            Debug.LogError(e);
                            throw;
                        }

                    }
                }
                
                phrase.Key = row[15];
                  
                phrase.Name = row[1];
                
                double.TryParse(row[2], out double cost);
                phrase.Cost = cost;
                
                Enum.TryParse(row[3],out BuildingName buildingName);
                phrase.BuildingKey = buildingName;
                
                Enum.TryParse(row[4],out SpriteName sprite);
                phrase.SpriteName = sprite;
                
                phrase.Description = row[12];
                
                int.TryParse(row[13], out int district);
                phrase.DistrictNeed = district;
                
                newPhrases.Add(phrase);
            }
            return newPhrases;
        }
    }
}