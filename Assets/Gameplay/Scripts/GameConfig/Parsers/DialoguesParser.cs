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
using UI.Scripts.DialogWindow;
using Unity.VisualScripting;
using UnityEngine;
using yutokun;

namespace Gameplay.Configs
{
    public class DialoguesParser : IParser
    {
        public object ParseObject(string csvString)
        {
            var newPhrases = new List<DialogPhrase>();
            var sheet = CSVParser.LoadFromString(csvString);

            for (int i = 0; i < sheet.Count; i++)
            {
                var row = sheet[i];

                var phrase = new DialogPhrase();

                if (i < 1) continue;
                
                phrase.Phrase = row[4];
                phrase.SpeakerName = row[3];
                
                Enum.TryParse(row[2], out SpriteName spriteName);
                phrase.DialoguePicture = spriteName;

                if (phrase.Phrase == String.Empty) continue;
                
                Enum.TryParse(row[1], out DialogueName dialogueName);
                phrase.DialogueName = dialogueName;
                
                newPhrases.Add(phrase);
            }

            return newPhrases;
        }
    }
}