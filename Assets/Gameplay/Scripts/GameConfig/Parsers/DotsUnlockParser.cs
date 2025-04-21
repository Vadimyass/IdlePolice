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
    public class DotsUnlockParser : IParser
    {
        public object ParseObject(string csvString)
        {
            var newPhrases = new List<DotsUnlockConfigInitializer>();
            var sheet = CSVParser.LoadFromString(csvString);
            for (int i = 0; i < sheet.Count; i++)
            {
                var row = sheet[i];

                if (i < 1) continue;

                var info = new DotsUnlockConfigInitializer();
                
                int.TryParse(row[2], out int level);
                info.Level = level;
                int.TryParse(row[4], out int district);
                info.District = district;

                Enum.TryParse(row[6],out PointType pointType);
                info.PointType = pointType;
                
                newPhrases.Add(info);
            }
            return newPhrases;
        }
       
    }
}