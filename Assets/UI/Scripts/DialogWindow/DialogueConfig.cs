using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Utils;
using MyBox;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace UI.Scripts.DialogWindow
{
    public class DialogueConfig : IConfig
    {
        private List<DialogPhrase> _dialogInitializers;
       
        public void LoadConfig(string sheetName)
        {
            _dialogInitializers = DataController.ReadSheetFromJson<DialogPhrase>(sheetName);
        }
        
        public DialogPhrase GetDialogInitializerByDialogName(DialogueName dialogueName)
        {
            return _dialogInitializers.FirstOrDefault(x => x.DialogueName== dialogueName);
        }
        
    }
    
    public enum DialogueName
    {
        tutorial_phrase_1,
        tutorial_phrase_2,
        tutorial_phrase_3,
        tutorial_phrase_4,
        tutorial_phrase_5,
        tutorial_phrase_6,
        tutorial_phrase_7,
        tutorial_phrase_8,
        tutorial_phrase_9,
        tutorial_phrase_10,
        tutorial_phrase_11,
        tutorial_phrase_12,
    }
}