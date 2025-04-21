using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Loaders;
using Gameplay.Scripts.Tutorial;
using Gameplay.Scripts.Utils;
using Tutorial;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Managers
{
    public class TutorialManager : MonoBehaviour
    {

        private readonly TutorialServiceLocator _serviceLocator = new TutorialServiceLocator();

        private TutorialFactory _tutorialFactory;
        private TutorialContextObjectConfig _tutorialContextObjectConfig;
        private IResourceLoader _resourceLoader;
        private DiContainer _diContainer;
        private SignalBus _signalBus;
        private UIManager _uiManager;
        private InputController _inputController;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private TutorialRunner _tutorialRunner;
        private CameraController _cameraController;

        [Inject]
        public void Construct(
            DiContainer diContainer, 
            TutorialConfig tutorialConfig, 
            TutorialContextObjectConfig tutorialContextObjectConfig,
            IResourceLoader resourceLoader,
            SignalBus signalBus,
            UIManager uiManager,
            InputController inputController,
            bool isTutorialActive,
            PlayerPrefsSaveManager playerPrefsSaveManager,
            CameraController cameraController)
        {
            _cameraController = cameraController;
            _playerPrefsSaveManager = playerPrefsSaveManager;
            _inputController = inputController;
            _tutorialFactory = new TutorialFactory(diContainer, tutorialConfig);
            _tutorialContextObjectConfig = tutorialContextObjectConfig;
            _diContainer = diContainer;
            _resourceLoader = resourceLoader;
            _signalBus = signalBus;
            _uiManager = uiManager;
            //_playerPrefsSaveManager.PrefsData.TutorialModel.SetTutorialSkip(!isTutorialActive);
        }
        
        public async UniTask Init()
        {
            _serviceLocator.SetService(TutorialTargetSingletonResolver.Container.Value);
            _serviceLocator.SetService<ITutorialUpdateLoop>(gameObject.AddComponent<UnityTutorialUpdateObserver>());
            _serviceLocator.SetService(_signalBus);
            _serviceLocator.SetService(_uiManager);
            _serviceLocator.SetService(_inputController);

            foreach (var data in _tutorialContextObjectConfig.TutorialObjects)
            {
                var tutorialObject = await _resourceLoader.LoadResourceAsync<GameObject>(data.AssetReference);
                var prefab = _diContainer.InstantiatePrefab(tutorialObject, transform);
                prefab.SetActive(false);
                _serviceLocator.SetService(data.TutorialObjectType.Type, prefab.GetComponent(data.TutorialObjectType.Type));
            }
        }

        private async void StartTutorial(ITutorial tutorial)
        {
            if (tutorial == null || _tutorialRunner != null) return;

            Debug.LogError($"[TutorialManager] Starting of tutorial {tutorial}.");
            
            _tutorialRunner = GetTutorial();
            
            _tutorialRunner.SetTutorial(tutorial, _serviceLocator);
            _tutorialRunner.NextStep();

            await UniTask.WaitWhile(() => _tutorialRunner.IsActiveTutorial);
            _tutorialRunner = null;
        }

        public async void SkipTutorial()
        {
            SendAnalytics();
            _playerPrefsSaveManager.PrefsData.TutorialModel.SetTutorCompleted(TutorialType.WelcomeTutorial);

            _playerPrefsSaveManager.PrefsData.TutorialModel.SetTutorialSkipped(true);

            _cameraController.EnabledLeanDragCamera(true);
            _cameraController.EnabledLeanCameraZoom(true);
            _playerPrefsSaveManager.DisableSave();
            _playerPrefsSaveManager.ForceSave();
            
            
            
            await UniTask.Delay(TimeSpan.FromMilliseconds(30));
            //SceneManagement.RestartCurrentSceneAsync();
            SceneOrder.OrderScene("FortScene");
            SceneManagement.LoadBootScene();
        }

        private void SendAnalytics()
        {
            //AnalyticsManager.LogAppMetricaEvent(EventMetricaName.initial_tutor_skip_yes,false,
            //   (EventMetricaParameterName.skip_step_number,_playerPrefsSaveManager.AnalyticsInfoModel.CurrentTutorialStep));
        }

        private TutorialRunner GetTutorial()
        {
            var tutorialRunner = new TutorialRunner();
            tutorialRunner.Configurate();

            return tutorialRunner;
        }

        public void StartTutorial(TutorialType tutorialType)
        {
            StartTutorial(_tutorialFactory.GetContextTutorial(tutorialType));
        }

        public bool TryStartTutorial(TutorialType tutorialType)
        {
            if (_tutorialRunner is { IsActiveTutorial: true })
            {
                return false;
            }

            _playerPrefsSaveManager.PrefsData.TutorialModel.SetTutorialSkip(false);
            
            if (!_playerPrefsSaveManager.PrefsData.TutorialModel.IsContextTutorDone(tutorialType))
            {
                StartTutorial(_tutorialFactory.GetContextTutorial(tutorialType));
                return true;
            }

            return false;
        }
    }
}