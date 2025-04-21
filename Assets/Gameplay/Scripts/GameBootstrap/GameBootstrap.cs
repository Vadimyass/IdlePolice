

using System;
using Agents;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.OrderManaging;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Loaders;
using Gameplay.Scripts.Missions;
using Gameplay.Scripts.Utils;
using Lean.Touch;
using Managers;
using MyBox;
using UI;
using UI.Scripts.MainScreen;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.GameBootstrap
{
    public class GameBootstrap : MonoBehaviour
    {
        private BigD.Config.GameConfig _gameConfig;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private DiContainer _diContainer;
        private AgentsManager _agentsManager;
        private UIManager _uiManager;
        private LevelController _levelController;
        private LevelConfig _levelConfig;
        private OrderManager _orderManager;
        private CameraController _cameraController;
        private MissionController _missionController;
        private BaseUpgradesController _baseUpgradesController;
        private SignalBus _signalBus;
        private IResourceLoader _resourceLoader;
        private TutorialManager _tutorialManager;


        [Inject]
        private void Construct(
            BigD.Config.GameConfig gameConfig,
            PlayerPrefsSaveManager playerPrefsSaveManager,
            DiContainer diContainer,
            AgentsManager agentsManager,
            UIManager uiManager,
            LevelController levelController,
            LevelConfig levelConfig,
            OrderManager orderManager,
            CameraController cameraController,
            MissionController missionController,
            BaseUpgradesController baseUpgradesController,
            SignalBus signalBus,
            IResourceLoader resourceLoader,
            TutorialManager tutorialManager)
        {
            _tutorialManager = tutorialManager;
            _resourceLoader = resourceLoader;
            _signalBus = signalBus;
            _baseUpgradesController = baseUpgradesController;
            _missionController = missionController;
            _cameraController = cameraController;
            _orderManager = orderManager;
            _levelConfig = levelConfig;
            _levelController = levelController;
            _uiManager = uiManager;
            _agentsManager = agentsManager;
            _diContainer = diContainer;
            _playerPrefsSaveManager = playerPrefsSaveManager;
            _gameConfig = gameConfig;
        }
        
        private async void Start()
        {
            Application.targetFrameRate = 60;
            FireSignal(0.5f);
            await _playerPrefsSaveManager.Init();
            //_audioManager.Init();
            await _gameConfig.LoadConfigs(_diContainer);
            FireSignal(0.7f);
           
            _cameraController.CreateNormalCamera();
            _baseUpgradesController.Init();
            _levelController.Init(_playerPrefsSaveManager,_levelConfig,_diContainer,_resourceLoader, transform);
            await _levelController.SetLevel();
            await _tutorialManager.Init();
            FireSignal(0.8f);
            
            _agentsManager.Init();
            _missionController.Init();
            _orderManager.Init();
            _cameraController.FocusOnBuilding(_levelController.CurrentLevel.GetAllBuiltBuildings()[0].transform);
            //_localizationManager.SetLanguageOnStart();
            //_iapManager.InitializePurchasing();
           //_cameraController.CreateNormalCamera();
           
            FireSignal(1);
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            
            _uiManager.Show<MainScreenController>();
        }
        

        private void FireSignal(float value)
        {
            _signalBus.Fire(new LoadingProgressSignal(value));
        }

        private void OnDestroy()
        {
            
        }
    }
}