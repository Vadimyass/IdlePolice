using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.LockedZoneManagement;
using Gameplay.Scripts.Missions.MissionsType;
using Gameplay.Scripts.Utils;
using MyBox;
using TypeReferences;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Scripts.Missions
{
    public class MissionController
    {
        private List<Mission> _missions = new List<Mission>();
        private SignalBus _signalBus;
        private PlayerPrefsSaveManager _prefsSaveManager; 
        private BigD.Config.GameConfig _gameConfig;
        private UIManager _uiManager;
        private AudioManager _audioManager;
        private LevelController _levelController;

        public Action OnCheckMissionAction;

        public int BaseID => _levelController.CurrentLevel.Level;
        public MissionsModel MissionsModel => _prefsSaveManager.PrefsData.MissionsModel;
        
        [Inject]
        private void Construct(SignalBus signalBus, UIManager uiManager, AudioManager audioManager, LevelController levelController, PlayerPrefsSaveManager prefsSaveManager, BigD.Config.GameConfig gameConfig)
        {
            _levelController = levelController;
            _audioManager = audioManager;
            _uiManager = uiManager;
            _gameConfig = gameConfig;
            _prefsSaveManager = prefsSaveManager;
            _signalBus = signalBus;
            }
        
        private void OnFinishBuilding(BuildBuildingSignal signal)
        {
            CheckCompleteSignals<BuildBuildingMission>();
        }
        
        private void OnUpgradeObject(UpgradeBuildingSignal signal)
        {
            CheckCompleteSignals<UpgradeObjectMission>();
        }
        
        private void OnUpgradeBase(BaseUpgradeSignal signal)
        {
            CheckCompleteSignals<UpgradeBaseMission>();
        }
        
        private void OnProcessFinished(ProcessFinishSignal signal)
        {
            var mission = _missions.FirstOrDefault();
            if (mission != null)
            {
                if (mission.GetType() == typeof(TargetProcessMission))
                {
                    ((TargetProcessMission)mission).TryIncreaseProgress(signal.BuildingProcessName);
                    CheckCompleteSignals<TargetProcessMission>();
                }
            }
        }
        
        private void OnOpenDistrict(OpenDistrictZoneSignal signal)
        {
            CheckCompleteSignals<OpenCityDistrictMission>();
        }

        public void Init()
        {
            SetAllMissions();
            CheckAllMissions();
            
            _signalBus.Subscribe<UpgradeBuildingSignal>(OnUpgradeObject);
            _signalBus.Subscribe<BuildBuildingSignal>(OnFinishBuilding);
            _signalBus.Subscribe<BaseUpgradeSignal>(OnUpgradeBase);
            _signalBus.Subscribe<ProcessFinishSignal>(OnProcessFinished);
            _signalBus.Subscribe<OpenDistrictZoneSignal>(OnOpenDistrict);
        }

        public IReadOnlyList<Mission> GetListOfMissions()
        {
            var completed = _missions.Where(x => x.IsComplete == true).ToList();
            var unCompleted = _missions.Where(x => x.IsComplete == false).ToList();
            completed.AddRange(unCompleted);
            return completed;
        }

        public IReadOnlyList<Mission> GetListOfUncompleteMissions()
        {
            return _missions.Where(x => x.IsComplete == false).ToList();
        }
        
        public IReadOnlyList<Mission> GetListOfCompleteMissions()
        {
            return _missions.Where(x => x.IsComplete == true).ToList();
        }
        
        private void CheckAllMissions()
        {
            CheckCompleteSignals<BuildBuildingMission>();
            CheckCompleteSignals<UpgradeObjectMission>();
        }

        private void CheckCompleteSignals<T>() where T : Mission
        {
            var mission = GetFirstMission();
            _signalBus.Fire(new MissionCheckSignal());
            
            if(mission == null) return;

            if (mission.GetType() == typeof(T))
            {
                if (mission.CheckComplete() == true)
                {
                    _audioManager.PlaySound(TrackName.Mission_Complete);
                    mission.Complete();
                    _signalBus.Fire(new MissionCompleteSignal());
                }
            }

        }

        public Mission GetFirstMission()
        {
            var mission = _missions.FirstOrDefault(x => x.MissionSaveInfo.IsCompleted == false);
            if (mission != null)
            {
                if (mission.CheckComplete() == true)
                {
                    _audioManager.PlaySound(TrackName.Mission_Complete);
                    mission.Complete();
                    _signalBus.Fire(new MissionCompleteSignal());
                }
            }

            return mission;
        }

        public void CompleteMission(Mission mission)
        {
            _missions.Remove(mission);
            MissionsModel.CompleteMission(BaseID, mission.MissionConfigInitializer.NameKey);
        }
        

        public void GetRewardFromMission(Mission mission)
        {
            mission.GetReward();
            _prefsSaveManager.ForceSave();
            
            /*var nextMission = _gameConfig.MissionConfig.GetNextMission(BaseID, mission.MissionConfigInitializer.PhraseKey).PhraseKey;
            while (MissionsModel.CheckMissionDuplicate(BaseID, nextMission) == true)
            {
                nextMission = _gameConfig.MissionConfig.GetNextMission(BaseID, nextMission).PhraseKey;
            }
            SetMission(nextMission, true);*/
            
        }

        private void SetAllMissions()
        {
            _missions = new List<Mission>();
            var missionInfo = MissionsModel.GetMissionsList(BaseID);
            var currentMissions = missionInfo.CurrentMissions;
            if (currentMissions.Count == 0)
            {
                var missionsInitializers = _gameConfig.MissionsConfig.GetMissionsForBase(BaseID);
                var currentMissionsKeys = missionsInitializers.Select(x => x.NameKey).ToList();
                foreach (var mission in currentMissionsKeys)
                {
                    var missionSaveInfo = new MissionSaveInfo();
                    missionSaveInfo.Key = mission;
                    currentMissions.Add(missionSaveInfo);
                    
                    SetMission(missionSaveInfo, true);
                }
                return;
            }
            
            foreach (var mission in currentMissions)
            {
                SetMission(mission);
            }
        }

        private void SetMission(MissionSaveInfo missionID, bool isSave = false)
        {
            Mission missionEntity;
            var missionInitializer = _gameConfig.MissionsConfig.GetItemByKey(BaseID, missionID.Key);

            if (missionInitializer.BuildBuildingValue.Value != 0)
            {
                missionEntity = new BuildBuildingMission();
            }
            else if (missionInitializer.UpgradeBuildingValue.Value != 0)
            {
                missionEntity = new UpgradeObjectMission();
            }
            else if (missionInitializer.TargetUpgradeName != string.Empty)
            {
                missionEntity = new UpgradeBaseMission();
            }
            else if (missionInitializer.ProcessBuildingValue.Value != 0)
            {
                missionEntity = new TargetProcessMission();
            }
            else if (missionInitializer.NumberOfDistrictOpen != 0)
            {
                missionEntity = new OpenCityDistrictMission();
            }
            else
            {
                return;
            }

            missionEntity.Init(missionInitializer, missionID, _prefsSaveManager, _levelController);
            _missions.Add(missionEntity);
            if (isSave)
            {
                MissionsModel.TryAddCurrentMission(BaseID, missionID.Key);
            }
            CheckAllMissions();
        }
    }
}