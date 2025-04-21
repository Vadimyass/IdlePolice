using Gameplay.Configs;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Utils;
using UnityEngine;

namespace Gameplay.Scripts.Missions
{
    public abstract class Mission
    {
        protected PlayerPrefsSaveManager _prefsSaveManager;
        private bool _isComplete;
        public bool IsComplete => _isComplete;
        protected MissionConfigInitializer _missionConfigInitializer;
        protected LevelController _levelController;
        protected int _progress;
        public MissionSaveInfo MissionSaveInfo => _missionSaveInfo;
        protected MissionSaveInfo _missionSaveInfo;

        public MissionConfigInitializer MissionConfigInitializer => _missionConfigInitializer;
        
        public void Init(MissionConfigInitializer missionConfigInitializer, MissionSaveInfo missionSaveInfo, PlayerPrefsSaveManager prefsSaveManager, LevelController levelController)
        {
            _missionSaveInfo = missionSaveInfo;
            _missionConfigInitializer = missionConfigInitializer;
            _levelController = levelController;
            _prefsSaveManager = prefsSaveManager;
        }
        
        public abstract bool CheckComplete();
        public abstract int GetCurrentProgress();
        public abstract int GetGoal();

        public void Complete()
        {
            _isComplete = true;
        }
        
        
        public void GetReward()
        {
            switch (_missionConfigInitializer.CurrencyValue.Key)
            {
                case CurrencyUIType.Dollar:
                    _prefsSaveManager.PrefsData.CurrenciesModel.ChangeMoney(_missionConfigInitializer.CurrencyValue.Value, _levelController.CurrentLevel.Level);
                    break;
                case CurrencyUIType.Crystal:
                    _prefsSaveManager.PrefsData.CurrenciesModel.IncreaseCrystal((int)_missionConfigInitializer.CurrencyValue.Value);
                    break;
            }
            
            _missionSaveInfo.IsCompleted = true;
        }
    }
}