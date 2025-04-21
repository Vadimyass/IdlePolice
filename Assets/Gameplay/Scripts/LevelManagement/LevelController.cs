using Cysharp.Threading.Tasks;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Loaders;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.LevelManagement
{
    public class LevelController
    {
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private LevelConfig _levelConfig;

        private LevelBehaviour _currentLevel;
        public LevelBehaviour CurrentLevel => _currentLevel;
        private DiContainer _container;
        private IResourceLoader _resourceLoader;
        private Transform _root;

        public void Init(PlayerPrefsSaveManager playerPrefsSaveManager,LevelConfig levelConfig,DiContainer container,IResourceLoader resourceLoader,Transform root)
        {
            _root = root;
            _resourceLoader = resourceLoader;
            _container = container;
            _levelConfig = levelConfig;
            _playerPrefsSaveManager = playerPrefsSaveManager;
        }


        public async UniTask SetLevel()
        {
            var levelAsset = _levelConfig.GetLevelByIndex(_playerPrefsSaveManager.PrefsData.LevelInfoModel.CurrentLevelIndex);
            var instantiatedLevel = await _resourceLoader.InstantiateAsync(levelAsset,_root);
            _currentLevel = instantiatedLevel.GetComponent<LevelBehaviour>();
            _container.InjectGameObject(_currentLevel.gameObject);
            await _currentLevel.Init();
        }
    }
}