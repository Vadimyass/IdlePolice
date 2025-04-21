using System;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Utils;
using MyBox;
using UI;
using UI.Scripts.LoadingScreen;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Scripts.GameBootstrap
{
    public class LoadingBootstrap : MonoBehaviour
    {
        [Scene] [SerializeField] private string _gameplayScene;
        [Scene] [SerializeField] private string _bootScene;
        [SerializeField] private GameObject _bootLoadingScreen;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private UIManager _uiManager;
        private LocalizationManager _localizationManager;
        private DiContainer _diContainer;
        private BigD.Config.GameConfig _gameConfig;
        private SignalBus _signalBus;

        private bool _isGameLoaded;

        [Inject]
        private void Construct(PlayerPrefsSaveManager playerPrefsSaveManager,UIManager uiManager,LocalizationManager localizationManager,BigD.Config.GameConfig gameConfig,DiContainer diContainer,SignalBus signalBus)
        {
            _signalBus = signalBus;
            _gameConfig = gameConfig;
            _diContainer = diContainer;
            _localizationManager = localizationManager;
            _uiManager = uiManager;
            _playerPrefsSaveManager = playerPrefsSaveManager;
        }

        private async void Start()
        {
            await _playerPrefsSaveManager.Init();
            await _gameConfig.LoadConfigs(_diContainer);
            _localizationManager.SetLanguageOnStart();
            await _uiManager.Show<LoadingScreenController>();
            _uiManager.SetCanvasSortOrder(999);
            _bootLoadingScreen.SetActive(false);


            await LoadSceneAsync(_gameplayScene);


            await UniTask.WaitUntil(() => LoadingScreenController.OnLoadingEnded);
            UnloadSceneAsync();
        }



        async UniTask LoadSceneAsync(string sceneToLoad)
        {
            var loadOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            loadOperation.allowSceneActivation = false;

            
            while (!loadOperation.isDone)
            {
                if (loadOperation.progress >= 0.9f)
                {
                    loadOperation.allowSceneActivation = true;
                }
                await UniTask.Yield(); 
            }
        }

        private async UniTask UnloadSceneAsync()
        {
            if (!string.IsNullOrEmpty(_bootScene))
            {
                await SceneManager.UnloadSceneAsync(_bootScene);
            }
        }
    }
}