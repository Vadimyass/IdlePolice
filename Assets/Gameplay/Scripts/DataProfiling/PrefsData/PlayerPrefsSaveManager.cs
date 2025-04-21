using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Utils;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.DataProfiling.PrefsData
{
    public class PlayerPrefsSaveManager
    {
        public PlayerPrefsData PrefsData { get; private set; }

        /*public AnalyticsInfoModel AnalyticsInfoModel => _analyticsInfoModel;
        private AnalyticsInfoModel _analyticsInfoModel = new();*/
        
        public Dictionary<int,long> PauseTimeStamp => _pauseTimeStamp;

        [JsonProperty] private Dictionary<int, long> _pauseTimeStamp;
        
        private bool isDisabledSaves = false;
        private DiContainer _container;

        private bool _isInitialized;

        public PlayerPrefsSaveManager(DiContainer container)
        {
            _container = container;
        }

        public async UniTask Init()
        {
            if(_isInitialized) return;
            
            PrefsData = await DataController.ReadUserDataFromFileAsync() ?? new PlayerPrefsData();
            _pauseTimeStamp = await DataController.ReadTimeDataFromFileAsync() ?? new ()
            {
                {0,0},
                {1,0},
                {2,0}
            };
            
            /*_analyticsInfoModel = await DataController.ReadAnalyticsDataFromFileAsync() ?? new AnalyticsInfoModel();
            
            _analyticsInfoModel.Initialize();
            _container.Inject(_analyticsInfoModel);*/
            
            PrefsData.Initialize(_container);

            _isInitialized = true;
            await UniTask.CompletedTask;
        }

        public void DisableSave()
        {
            isDisabledSaves = !isDisabledSaves;
        }

        public void ForceSave()
        {
            BigDDebugger.LogError("save");
            if(isDisabledSaves) return;
            DataController.SaveUserDataToFileAsync(PrefsData);
            //PlayerPrefs.SetString(LocalDataParam, JsonConvert.SerializeObject(PrefsData));
        }
        
        public void ForceSaveTime()
        {
            if(isDisabledSaves) return;
            DataController.SaveTimeDataToFileAsync(_pauseTimeStamp);
        }

        public void ForceAnalyticsSave()
        {
            //DataController.SaveAnalyticsDataToFileAsync(_analyticsInfoModel);
        }

        
        public static void DeleteData()
        {
            DataController.ResetProgress();
        }
    }
}