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
    public class OfficersLevelParser : IParser
    {
        public object ParseObject(string csvString)
        {
            var newPhrases = new List<OfficerLevelInfoConfigInitializator>();
            var sheet = CSVParser.LoadFromString(csvString);
            for (int i = 0; i < sheet.Count; i++)
            {
                var row = sheet[i];

                if (i < 1) continue;

                var info = new OfficerLevelInfoConfigInitializator();
                
                int.TryParse(row[0], out int level);
                info.Level = level;
                double.TryParse(row[1], out double donutCost);
                info.DonutCost = donutCost;
                int.TryParse(row[2], out int cardCost);
                info.UpgradeCardCost = cardCost;
                double.TryParse(row[3], out double incomeMult);
                info.IncomeMultiplier = incomeMult;
                double.TryParse(row[4], out double fightPower);
                info.FightPower = fightPower;
                double.TryParse(row[5], out double hp);
                info.HP = hp;
                Enum.TryParse(row[6],out OfficerRarity rarity);
                info.OfficerRarity = rarity;
                
                newPhrases.Add(info);
            }
            return newPhrases;
        }
       
    }
}