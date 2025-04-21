using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Configs
{
    public class LocalizationManager : MonoBehaviour
    {
        private SystemLanguage _currentLanguage;
        public Action ChangeLanguageAction;
        private PlayerPrefsSaveManager _playerPrefsData;
        private BigD.Config.GameConfig _gameConfig;

        private int CurrentIndexLanguage => _gameConfig.LocalizationConfig.GetIndexOfLanguage(_currentLanguage);

        [Inject]
        private void Construct(PlayerPrefsSaveManager playerPrefsData,BigD.Config.GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _playerPrefsData = playerPrefsData;
        }

        public void SetLanguageOnStart()
        {
            _currentLanguage = _playerPrefsData.PrefsData.SettingsModel.GetCurrentLanguageIndex();
        }
        
        public Dictionary<SystemLanguage,string> GetAvailableLanguages()
        {
            var availableSystemLanguages = _playerPrefsData.PrefsData.SettingsModel.AvailableSystemLanguages;
            var languages = new Dictionary<SystemLanguage,string>();
            
            foreach (var language in availableSystemLanguages)
            {
                languages.Add(language,TryTranslate(language.ToString()));
            }
            return languages;
        }
        
        
        public void ChangeLanguage(SystemLanguage systemLanguage)
        {
            _currentLanguage = systemLanguage;
            _playerPrefsData.PrefsData.SettingsModel.SetLanguage(systemLanguage);
            ChangeLanguageAction?.Invoke();
        }

        public string GetCurrentLanguageTitle()
        {
            return _gameConfig.LocalizationConfig.GetCurrentLanguageTitle(_currentLanguage.ToString());
        }
        
        public string TryTranslate(string key)
        {
            var phrase = _gameConfig.LocalizationConfig.GetItemByKey(key);
            if (CurrentIndexLanguage == -1)//fix bug with initialization
            {
                return string.Empty;
            }
            if (phrase == null)
            {
                Debug.LogError("problem with key: " + key );
                return key;
            }
            return phrase.Pair[CurrentIndexLanguage] == string.Empty ? phrase.Pair[0] : phrase.Pair[CurrentIndexLanguage];
        }
    }
}