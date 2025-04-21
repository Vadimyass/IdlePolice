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
    public class BoxesInfoParser : IParser
    {
        public object ParseObject(string csvString)
        {
            var newPhrases = new List<BoxesConfigInitializator>();
            var sheet = CSVParser.LoadFromString(csvString);
            for (int i = 0; i < sheet.Count; i++)
            {
                var row = sheet[i];

                if (i < 1) continue;

                var info = new BoxesConfigInitializator();

                if (row[0] == string.Empty) break;
                
                Enum.TryParse(row[0], out GachaBoxType boxType);
                info.Key = boxType;
                double.TryParse(row[1],out double rareChance);
                info.RareChance = rareChance;
                double.TryParse(row[2],out double epicChance);
                info.EpicChance = epicChance;
                double.TryParse(row[3],out double legChance);
                info.LegendaryChance = legChance;
                int.TryParse(row[4],out int garanteeQuant);
                info.GaranteeQuantity = garanteeQuant;
                Enum.TryParse(row[5], out OfficerRarity officerRarity);
                info.Garantee = officerRarity;

                newPhrases.Add(info);
            }
            return newPhrases;
        }
       
    }
}