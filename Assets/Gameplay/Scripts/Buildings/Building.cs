using System;
using Agents;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Configs;
using Gameplay.OrderManaging;
using Gameplay.Scripts.Agents;
using Gameplay.Scripts.DataProfiling.Models;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Locker;
using Gameplay.Scripts.Missions;
using Gameplay.Scripts.Utils;
using Particles;
using TypeReferences;
using UI;
using UI.Huds.Scripts;
using UI.Scripts.UpgradeBuildingWindow;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Scripts.Buildings
{
    public class Building : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        [field:SerializeField] public Transform CarSlotTransform { get; private set; }
        [field: SerializeField] public Transform BusSlotTransform { get; private set; }
        [field: SerializeField] public string BuildingKey { get; private set; }
        [field: SerializeField] public AgentType CarAgentType { get; private set; }
        
        [field:SerializeField] public Building NextBuilding { get; private set; }
        
        [SerializeField] private Transform _buildObject;
        [SerializeField] private bool _isBuiltOnStart;
        [SerializeField] private BuildingName _buildingName;
        [SerializeField] private BuildingBusManager _buildingBusManager;
        [SerializeField] private BuildingCarManager _buildingCarManager;
        [SerializeField] private BuildingCarsHudContainer _hudCars;
        [SerializeField] private BuildingHudContainer _hudMoney;
        [SerializeField] private BuildingAnimation _buildingAnimation;
        [SerializeField] private BuildHud _buildHud;
        [SerializeField] private BuildingProcessName _buildingProcessName;
        [SerializeField] private BuildingPlus _buildingPlus;
        [SerializeField] private Transform _particleTransform;
        [Inherits(typeof(Order))] [SerializeField] private TypeReference _openOrderType;
        
        [SerializeField] private GroundType _groundType;

        private bool _isDragged;
        public bool IsAutomated { get; private set; }
        private UIManager _uiManager;
        public int Level { get; private set; } = 1;
        private BigD.Config.GameConfig _gameConfig;
        public BuildingConfigInitializator Info { get; private set; }

        public string OfficerSet { get; private set; } = string.Empty;

        private float _duration;
        private double _storedIncome;

        private double _incomeProgress;
        private Vector3 _startScale;
        
        public double RealIncome =>  GetRealIncome();
        public bool IsBuilt => _buildingSaveData.IsBuilt;
        public double RealDuration => _duration / (_upgradeBaseModifiers.BuildingSpeedMultiplier);
        private Action _takeIncomeAction;
        private PlayerPrefsSaveManager _prefsSaveManager;
        private BaseUpgradesController _baseUpgradesController;
        [SerializeField] private BuildingModifiers _upgradeBaseModifiers;
        private BuildingSaveData _buildingSaveData;
        private SignalBus _signalBus;
        private int _iterationQueueCount = 0;
        private int _iterationsReadyCount = 0;
        private CarAgent _carAgent;
        private LevelController _levelController;
        private CameraController _cameraController;
        private ParticleManager _particleManager;
        private bool _isGetIteration;
        private LockController _lockController;
        public UpgradeConfigInitializator CurrentStageInfo { get; private set; }

        public BuildingName BuildingName => _buildingName;
        public BuildingModifiers BuildingModifiers => _upgradeBaseModifiers;
        public Transform BuildObject => _buildObject;

        [Inject]
         private void Construct(UIManager uiManager, LockController lockController, SignalBus signalBus, ParticleManager particleManager, BigD.Config.GameConfig gameConfig, BaseUpgradesController baseUpgradesController, PlayerPrefsSaveManager prefsSaveManager,AgentsManager agentsManager,LevelController levelController,CameraController cameraController)
         {
             _lockController = lockController;
             _particleManager = particleManager;
             _cameraController = cameraController;
             _levelController = levelController;
             _signalBus = signalBus;
             _baseUpgradesController = baseUpgradesController;
             _prefsSaveManager = prefsSaveManager;
             _gameConfig = gameConfig;
             _uiManager = uiManager;
        }
        

        public async void Init()
        {
            _buildingSaveData = _prefsSaveManager.PrefsData.LevelInfoModel.GetBuildingData(_levelController.CurrentLevel.Level,BuildingKey, out bool firstTime);
            Debug.LogError(firstTime);
            _startScale = transform.localScale;
            Level = _buildingSaveData.Level;
            _storedIncome = _buildingSaveData.StoredIncome;
            _buildObject.gameObject.SetActive(false);
            Info = _gameConfig.UpgradeConfig.GetBuildingByKey(BuildingKey, _levelController.CurrentLevel.Level);
            _duration = _gameConfig.UpgradeConfig.GetItemByKey(BuildingKey, 1, _levelController.CurrentLevel.Level).Duration;
            _upgradeBaseModifiers = _baseUpgradesController.GetInfoForBuilding(_buildingName);

            _levelController.CurrentLevel.AddAvailableGroundType(_groundType);
            
            foreach (var infoUpgrade in Info.Upgrades)
            {
                if(infoUpgrade.Level > Level) break;
                
                if (infoUpgrade.AdditiveParamName == AdditiveParamName.Open_dots)
                {
                    if (infoUpgrade.AdditiveParamValue != PointType.Default)
                    {
                        _levelController.CurrentLevel.AddAvailablePointType(infoUpgrade.AdditiveParamValue);
                    }
                }
            }
            
            
            CurrentStageInfo = _gameConfig.UpgradeConfig.GetItemByKey(BuildingKey, Level, _levelController.CurrentLevel.Level);
            
            _buildingPlus.gameObject.SetActive(true);
            
            IncomeTick();

            _signalBus.Subscribe<BaseUpgradeSignal>(OnBaseUpgrade);
            _signalBus.Subscribe<BuildBuildingSignal>(OnAnotherBuildingBuilt);
            
            if (_hudMoney != null)
            {
                _hudMoney.Configurate(OfficerSet, 0, _storedIncome, TakeIncome, _gameConfig.UpgradeConfig.GetItemByKey(BuildingKey, Level, _levelController.CurrentLevel.Level).SpriteName, Info.BuildingType);
            }
            
            if (_hudCars != null)
            {
                _hudCars.Configurate(OfficerSet, Info.BuildingType, _storedIncome, TakeIncome);
                SetCarsCount();
            }
            
            if (IsBuilt == true || _isBuiltOnStart == true)
            {
                _buildHud.Hide();
                SetOfficer(_buildingSaveData.OfficerKey,true);
                Build(true, firstTime);
            }
            
            _buildingPlus.SetText(Info.Name);
        }

        private void OnAnotherBuildingBuilt(BuildBuildingSignal obj)
        {
            if(obj.BuildingName == BuildingName) return;
            
            if(NextBuilding == null || _buildingBusManager == null) return;
            
            BigDDebugger.LogError(obj.BuildingName, NextBuilding.BuildingName);
            
            if (obj.BuildingName == NextBuilding.BuildingName)
            {
                _buildingBusManager.ShowBus(true);
            }
        }

        public void TryShowBuildHud()
        {
            if(IsBuilt) return;
            GoToBuildWindow();
            gameObject.SetActive(true);
        }

        private void OnBaseUpgrade(BaseUpgradeSignal signal)
        {
            if (signal.BuildingName == BuildingName.All || signal.BuildingName == _buildingName)
            {
                _upgradeBaseModifiers = _baseUpgradesController.GetInfoForBuilding(_buildingName);
                ForceUpdateModificators();
            }
        }

        private void ForceUpdateModificators()
        {
            _buildingCarManager?.TryValidateSpeed();
            _buildingBusManager?.TryValidateSpeed();
        }

        public BuildingModifiers GetFullBuildingModifiers()
        {
            var modifiers = new BuildingModifiers();
            _upgradeBaseModifiers = _baseUpgradesController.GetInfoForBuilding(_buildingName);
            
            modifiers.SpeedMultiplier = _upgradeBaseModifiers.SpeedMultiplier;
            modifiers.IncomeMultiplier = _upgradeBaseModifiers.IncomeMultiplier;
            
            if (OfficerSet != string.Empty)
            {
                var officerRarity = _gameConfig.OfficersInfoConfig.GetItemByKey(OfficerSet).OfficerRarity;
                var officerModifier = _gameConfig.OfficersLevelsInfoConfig.GetItemByKey(officerRarity,
                    _prefsSaveManager.PrefsData.OfficersModel.GetSaveInfo(OfficerSet).Level).IncomeMultiplier;

                modifiers.IncomeMultiplier *= officerModifier;
            }

            return modifiers;
        }
        
        private async void IncomeTick()
        {
            if (_iterationQueueCount <= 0 || _isGetIteration == false)
            {
                _buildingAnimation?.PauseAnimation();
            }
            
            if (IsAutomated == false)
            {
                await UniTask.WaitUntil(() => _storedIncome == 0 || IsAutomated == true);
            }
            
            if (_isGetIteration == false && _iterationQueueCount > 0)
            {
                _isGetIteration = true;
                TakeIteration();
            }
            else if (_isGetIteration == false && _iterationQueueCount <= 0)
            {
                await UniTask.WaitWhile(() => _iterationQueueCount <= 0);
                
                TakeIteration();
                _isGetIteration = true;
            }
            
            _buildingAnimation?.StartAnimation();
            
            _incomeProgress++;
            while (_incomeProgress >= RealDuration)
            {
                _signalBus.Fire(new ProcessFinishSignal(_buildingProcessName));
                if (IsAutomated == false)
                {
                    _incomeProgress = RealDuration;
                    _storedIncome = RealIncome;
                    _buildingSaveData.StoredIncome = _storedIncome;
                    SaveData();
                    _isGetIteration = false;
                    _iterationsReadyCount++;
                    _hudMoney.UpdateInfo(1, _storedIncome, _iterationQueueCount, true, true);
                    _buildingAnimation?.PauseAnimation();
                    MoveToBus();
                    IncomeTick();
                    await UniTask.Delay(TimeSpan.FromSeconds(1));
                    return;
                }
                
                _incomeProgress -= RealDuration;
                _storedIncome += RealIncome;
                _buildingSaveData.StoredIncome = _storedIncome;
                _hudMoney.UpdateMoney(_storedIncome, true);
                SaveData();
                _isGetIteration = false;
                MoveToBus();
                _iterationsReadyCount++;
            }
                
            _hudMoney.UpdateInfo( _incomeProgress/RealDuration, _storedIncome,_iterationQueueCount, false, true);
            
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            
            IncomeTick();
        }

        public async void AddIteration(int count, CarAgent carTransform)
        {
            await _hudMoney.AddOrderUnit(carTransform);

            if (_iterationQueueCount >= 10)
            {
                return;
            }
            
            if (_iterationQueueCount == 0 && _isGetIteration == false && IsAutomated == false && _storedIncome <= 0)
            {
                _iterationQueueCount += count;
                return;
            }
            
            _iterationQueueCount += count;
            
            
            
            _hudMoney.SetQueueCount(_iterationQueueCount, false);
        }

        public async void TakeIteration()
        {
            _iterationQueueCount--;
            _hudMoney.SetQueueCount(_iterationQueueCount, false);
        }

        public async void MoveToBus()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            GiveBusIterations();
        }

        private async void GiveBusIterations()
        {
            if (_buildingBusManager.TryAddIteration())
            {
                var bus = _buildingBusManager.GetFirstBusAgent();
                if (bus.gameObject.activeSelf == true)
                {
                    await _hudMoney.RemoveOrderUnit(bus.transform);
                }
            }
        }

        public void TakeIncome()
        {
            _prefsSaveManager.PrefsData.CurrenciesModel.ChangeMoney(_storedIncome, _levelController.CurrentLevel.Level);
            _storedIncome = 0;
            SaveData();
            
            if (IsAutomated == false)
            {
                _incomeProgress = 0;
                _hudMoney?.UpdateMoney( 0,  false);
                _hudMoney?.SetProgressBarValueNoAnimation(0);
                _hudCars?.SetMoneyToMoneyHud(0);
                return;
            }
            
            _hudMoney?.UpdateMoney( 0, false);
            _hudCars?.SetMoneyToMoneyHud(0);
        }

        public void SetOfficer(string officerKey,bool isFromStart = false)
        {
            OfficerSet = officerKey;
            var levelOfficer = 0;

            if (officerKey != String.Empty)
            {
                levelOfficer = _prefsSaveManager.PrefsData.OfficersModel.GetSaveInfo(officerKey).Level;
            }

            if (_hudMoney != null)
            {
                _hudMoney.SetOfficer(OfficerSet);
            }

            if (_hudCars != null)
            {
                _hudCars.SetOfficer(OfficerSet, Info.BuildingType);
            }


            IsAutomated = Info.AutomateLvl <= levelOfficer;

            if (officerKey == String.Empty)
                IsAutomated = false;

            if (IsAutomated == true)
            {
                if (_hudMoney != null)
                {
                    _hudMoney.UpdateInfo(0, _storedIncome, _iterationQueueCount, false, false);
                }
            }

            _buildingSaveData.OfficerKey = officerKey;
            
            if(isFromStart) return;
            
            SaveData();
        }

        public void SetCarsCount()
        {
            if (_hudCars != null)
            {
                _hudCars.SetCarsCount(_buildingCarManager.GetCurrentAgentsCount());
            }
        }
        
        public void SaveData()
        {
            _prefsSaveManager.PrefsData.LevelInfoModel.SetBuildingData(_levelController.CurrentLevel.Level,_buildingSaveData);
            _prefsSaveManager.ForceSave();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isDragged = false;
        }

        public void Upgrade(bool toNextStage)
        {
            var seq = DOTween.Sequence();
            seq.Append(transform.DOScaleX(_startScale.x * 1.05f, 0.1f));
            seq.Append(transform.DOScaleX(_startScale.x, 0.1f));
            
            if (toNextStage)
            {
                _particleManager.PlayParticleInTransform(ParticleType.upgradeMilestone, _particleTransform,
                    Quaternion.identity);
                var nextStageByLevel = _gameConfig.UpgradeConfig.GetNextStageByLevel(BuildingKey, Level, _levelController.CurrentLevel.Level);
                if(nextStageByLevel == null) return;
                Level = nextStageByLevel.Level;
            }
            else
            {
                _particleManager.PlayParticleInTransform(ParticleType.upgradeBuilding, _particleTransform,
                    Quaternion.identity);
                Level++;
            }

            CurrentStageInfo = _gameConfig.UpgradeConfig.GetItemByKey(BuildingKey, Level, _levelController.CurrentLevel.Level);
            _buildingCarManager?.TryValidateCarsCount();
            _signalBus.Fire(new UpgradeBuildingSignal());
            
            SetCarsCount();

            if (CurrentStageInfo.AdditiveParamName == AdditiveParamName.Open_dots)
            {
                if (CurrentStageInfo.AdditiveParamValue != PointType.Default)
                {
                    _levelController.CurrentLevel.AddAvailablePointType(CurrentStageInfo.AdditiveParamValue);
                }
            }

            if (_hudMoney != null)
            {
                _hudMoney.SetSprite(CurrentStageInfo.SpriteName);
            }

            _buildingSaveData.Level = Level;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(_isDragged == true) return;
            if(eventData.used) return;
            
            if (IsBuilt == false)
            {
                GoToBuildWindow();
                return;
            }
            
            GoToUpgradeWindow();
        }

        public void GoToUpgradeWindow()
        {
            if(_lockController.HaveLock<OpenDistrictLocker>() || _lockController.HaveLock<OrderSpawnLocker>() ||_lockController.HaveLock<ShopLocker>() ||_lockController.HaveLock<UpgradeBaseLocker>()) return;
            
            _cameraController.FocusOnBuilding(transform, true);
            _uiManager.Show<UpgradeBuildingWindowController>(new UpgradeBuildingWindowArguments(this));
            var seq = DOTween.Sequence();
            seq.Append(transform.DOScaleX(_startScale.x * 1.1f, 0.1f));
            seq.Append(transform.DOScaleX(_startScale.x, 0.1f));
        }

        public void GoToBuildWindow()
        {
            _buildHud.Show(Info,() =>  Build());
        }
        

        private async UniTask Build(bool isFromStart = false, bool firstLaunch = true)
        {
            _buildingSaveData.IsBuilt = true;
            
            _buildingPlus.gameObject.SetActive(false);
            
            if (isFromStart == false)
            {
                _buildingAnimation.BuildAnimation();
                _signalBus.Fire(new BuildBuildingSignal(_buildingName));
                SaveData();
            }
            _levelController.CurrentLevel.SetAvailableOrderType(_openOrderType.Type);
            gameObject.SetActive(true);
            _particleManager.PlayParticleInPosition(ParticleType.poof, transform.position, Quaternion.identity, 40);
            _buildObject.gameObject.SetActive(true);
            
           
            if (_buildingBusManager != null)
            {
                await _buildingBusManager.Init(this,NextBuilding);
            }

            if (_buildingCarManager != null)
            {
                await _buildingCarManager.Init(this, firstLaunch);
            }

            
            SetCarsCount();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            _isDragged = true;
        }

        public double GetRealIncome()
        {
            var info = _gameConfig.UpgradeConfig.GetItemByKey(BuildingKey, Level, _levelController.CurrentLevel.Level);
            var income = info.Income;
            
            return income * GetFullBuildingModifiers().IncomeMultiplier;
        }

        public SpriteName GetCurrentUpgradeSpriteName()
        {
            return _gameConfig.UpgradeConfig.GetItemByKey(BuildingKey, Level, _levelController.CurrentLevel.Level).SpriteName;
        }

        public void OnClickOutside()
        {
            if(IsBuilt == true) return;
            _buildHud.OnClickOutside();
        }

        public void AddMoneyForCarHud(double money)
        {
            _storedIncome += money;
            _buildingSaveData.StoredIncome = _storedIncome;
            SaveData();
            _hudCars.SetMoneyToMoneyHud(_storedIncome);
        }
        
        public bool IsMaxLevel()
        {
            return Level == Info.Upgrades.Count;
        }

        public int GetCarsCount()
        {
            if (_buildingCarManager != null)
            {
                return _buildingCarManager.GetCurrentAgentsCount();
            }

            return 0;
        }
    }
    
    public enum BuildingProcessName
    {
        Arrest,
        Interrogate,
        File
    }
}