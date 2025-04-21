using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Scripts.Criminals;
using UI;
using UnityEngine;
using yutokun;

namespace Gameplay.Configs
{
    public class CriminalsParser : IParser
    {
        public object ParseObject(string csvString)
        {
        var newPhrases = new List<CriminalInitializator>();
            var sheet = CSVParser.LoadFromString(csvString);
            for (int i = 0; i < sheet.Count; i++)
            {
                var row = sheet[i];

                if (i < 1) continue;
                
                if(row[0] == string.Empty) continue;
                var criminalInit = new CriminalInitializator();
                
                if (Enum.TryParse(row[1], out CriminalNames criminalName))
                {
                    criminalInit.CriminalName = criminalName;
                }
                else
                {
                    Debug.LogError($"add specific {row[1]} criminal name to CriminalsNames");
                    continue;
                }
                
                if (Enum.TryParse(row[2], out SpriteName spriteName))
                {
                    criminalInit.CriminalIcon = spriteName;
                }
                else
                {
                    Debug.LogError($"add specific {row[2]} sprite name to CriminalsNames");
                    continue;
                }
                
                
                if (float.TryParse(row[3], out float duration))
                {
                    criminalInit.CriminalTime = duration;
                }
                
                newPhrases.Add(criminalInit);
            }

            return newPhrases;
        }
    }
}