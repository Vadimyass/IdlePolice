using System.Collections.Generic;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using UnityEngine;
using Zenject;

namespace Gameplay.Configs
{
    public class LocalizationConfig : IConfig
    {
        private List<DefaultConfigInitializer> _items = new ();
        private PlayerPrefsSaveManager _playerPrefsSaveManager;

        [Inject]
        private void Construct(PlayerPrefsSaveManager playerPrefsSaveManager)
        {
            _playerPrefsSaveManager = playerPrefsSaveManager;
        }
        
        public void LoadConfig(string sheetName)
        {
            _items = DataController.ReadSheetFromJson<DefaultConfigInitializer>(sheetName);
        }

        public DefaultConfigInitializer GetItemByKey(string key)
        {
            return _items.Find(x => x.Key == key);
        }
        
        public int GetIndexOfLanguage(SystemLanguage systemLanguage)
        {
            var index = _playerPrefsSaveManager.PrefsData.SettingsModel.AvailableSystemLanguages.IndexOf(systemLanguage);
            if (index == -1)
            {
                index = 1;
            }
            return index;
        }

        public string GetCurrentLanguageTitle(string key)
        {
            return GetItemByKey(key)?.Pair[0];
        }
    }
}