using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.DataProfiling.Models;
using Gameplay.Scripts.TimeManagement;
using Gameplay.Scripts.Utils;
using Zenject;

namespace Gameplay.Scripts.DataProfiling.PrefsData
{
    [Serializable]
    public class PlayerPrefsData
    {
        public TimerModel TimerModel => _timerModel;
        private TimerModel _timerModel = new();

        public LevelInfoModel LevelInfoModel => _levelInfoModel;
        private LevelInfoModel _levelInfoModel = new ();
        public SettingsModel SettingsModel => _settingsModel;
        private SettingsModel _settingsModel = new();
        
        public BaseUpgradesModel BaseUpgradesModel => _baseUpgradesModel;
        private BaseUpgradesModel _baseUpgradesModel = new();
        
        public СonsumablesInfoModel ConsumablesInfoModel => _consumablesModel;
        private СonsumablesInfoModel _consumablesModel = new();
        
        public CurrenciesModel CurrenciesModel => _currenciesModel;
        private CurrenciesModel _currenciesModel = new();
        
        public OfficersModel OfficersModel => _officersModel;
        private OfficersModel _officersModel = new();

        public MissionsModel MissionsModel => _missionsModel;
        private MissionsModel _missionsModel = new();
        
        public TutorialModel TutorialModel => _tutorialModel;
        private TutorialModel _tutorialModel = new();

        public async UniTask Initialize(DiContainer container)
        {
            foreach (var model in ReflectionUtils.GetFieldsOfType<IPlayerPrefsData>(this))
            {
                container.Inject(model);
                model.Initialize();
                await UniTask.CompletedTask;
            }
        }
    }
}