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
    public class OfficersInfoParser : IParser
    {
        public object ParseObject(string csvString)
        {
            var newPhrases = new List<OfficerConfigInitializator>();
            var sheet = CSVParser.LoadFromString(csvString);
            for (int i = 0; i < sheet.Count; i++)
            {
                var row = sheet[i];

                if (i < 1) continue;

                var info = new OfficerConfigInitializator();

                if (row[1] == string.Empty) break;
                
                info.Key = row[1];
                Enum.TryParse(row[2],out OfficerType type);
                info.OfficerType = type;
                Enum.TryParse(row[3],out OfficerRarity rarity);
                info.OfficerRarity = rarity;
                Enum.TryParse(row[4],out SpriteName spriteName);
                info.SpriteName = spriteName;

                newPhrases.Add(info);
            }
            return newPhrases;
        }
       
    }
}