using System;
using System.Collections.Generic;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Newtonsoft.Json;
using UnityEngine;

namespace Gameplay.Scripts.DataProfiling.PrefsData
{
    [Serializable]
    public class SettingsModel : IPlayerPrefsData
    {
        [JsonProperty] public bool IsSoundEnabled { get; private set; } = true;
        [JsonProperty] public bool IsVibrationEnabled { get; private set; } = true;
        [JsonProperty] public bool IsMusicEnabled { get; private set; } = true;
        [JsonProperty] public SystemLanguage LanguageIndex { get; private set; }
        [JsonProperty] public bool IsLanguageSetted { get; private set; }
        
        [JsonProperty] public bool IsRateUsAccepted { get; private set; }
        
        [JsonIgnore] private List<SystemLanguage> _availableLanguages = new ()
        {
            SystemLanguage.English,
            SystemLanguage.Ukrainian,
            SystemLanguage.French,
            SystemLanguage.Italian,
            SystemLanguage.Spanish,
            SystemLanguage.German,
            SystemLanguage.Russian,
            SystemLanguage.Polish,
            SystemLanguage.Portuguese,
            SystemLanguage.Indonesian,
            SystemLanguage.Japanese,
            SystemLanguage.Turkish,
            SystemLanguage.Korean,
            SystemLanguage.Chinese,
        };
        
        [JsonIgnore] public List<SystemLanguage> AvailableSystemLanguages => _availableLanguages;


        public void SetRateUsAccept(bool isAccept)
        {
            IsRateUsAccepted = isAccept;
        }
        
        public SystemLanguage GetCurrentLanguageIndex()
        {
            if (IsLanguageSetted)
            {
                return LanguageIndex; 
            }

            if (_availableLanguages.Contains(Application.systemLanguage))
            {
                return Application.systemLanguage;
            }

            return SystemLanguage.English;
        }

        public void SetLanguage(SystemLanguage language)
        {
            IsLanguageSetted = true;
            LanguageIndex = language;
        }

        public void Initialize()
        {
            
        }

        public bool IsSettingEnabled(SettingType settingType)
        {
            switch (settingType)
            {
                case SettingType.Music:
                {
                    return IsMusicEnabled;
                    break;
                }
                case SettingType.Sound:
                {
                    return IsSoundEnabled;
                    break;
                }
                case SettingType.Vibration:
                {
                    return IsVibrationEnabled;
                    break;
                }
            }
            return false;
        }

        public void SetSetting(bool setting, SettingType settingType)
        {
            switch (settingType)
            {
                case SettingType.Music:
                {
                    IsMusicEnabled = setting;
                    break;
                }
                case SettingType.Sound:
                {
                    IsSoundEnabled = setting;
                    break;
                }
                case SettingType.Vibration:
                {
                    IsVibrationEnabled = setting;
                    break;
                }
            }
        }
    }
}