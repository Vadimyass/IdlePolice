using System;
using Audio;
using BigD.Config;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Utils;
using UI.Scripts.ChoseLevelWindow;
using UI.UIUtils;
using Zenject;

namespace UI.Scripts.TravelWindow
{
    public class TravelWindowController : UIScreenController<TravelWindow>
    {
        private UIManager _uiManager;
        private LevelController _levelController;
        private GameConfig _gameConfig;
        private float _cost;
        private PlayerPrefsSaveManager _prefsSaveManager;

        [Inject]
        private void Construct(UIManager uiManager, LevelController levelController, GameConfig gameConfig, PlayerPrefsSaveManager prefsSaveManager)
        {
            _prefsSaveManager = prefsSaveManager;
            _gameConfig = gameConfig;
            _levelController = levelController;
            _uiManager = uiManager;
        }

        public override async UniTask Display(UIArguments arguments)
        {
            await base.Display(arguments);
            
            View.CloseButton.onClick.RemoveAllListeners();
            View.ChoseLevelButton.onClick.RemoveAllListeners();
            View.BuyButton.onClick.RemoveAllListeners();
            View.TravelButton.onClick.RemoveAllListeners();

            View.ChoseLevelButton.onClick.AddListener((() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.Show<ChoseLevelWindowController>();
            }));
            View.CloseButton.onClick.AddListener((() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.HideLast();
            }));
            View.BuyButton.onClick.AddListener(BuyAccess);
            View.TravelButton.onClick.AddListener(MoveToNextLevel);

            var currentLevel = _levelController.CurrentLevel.Level;

            _cost = _gameConfig.EconomyConfig.GetItemByKey(EconomyEnum.New_LVL_unlock_cost, currentLevel+1);
            View.CostText.text = TextMeshProUtils.ConvertBigDoubleToText(_cost);
            View.CostText2.text = TextMeshProUtils.ConvertBigDoubleToText(_cost);
            
            View.CurrentCityImage.sprite = View.Sprites[currentLevel];
            View.NextCityImage.sprite = View.Sprites[currentLevel+1];

            View.TravelButton.gameObject.SetActive(false);

            View.BuyButton.gameObject.SetActive(_prefsSaveManager.PrefsData.CurrenciesModel.BasesMoney[currentLevel] >= _cost);
            View.UnActiveBuyButton.gameObject.SetActive(_prefsSaveManager.PrefsData.CurrenciesModel.BasesMoney[currentLevel] < _cost);
            
            if (_prefsSaveManager.PrefsData.LevelInfoModel.OpenedLevelIndex > currentLevel)
            {
                View.TravelButton.gameObject.SetActive(true);
            }
            
            View.OpenAllZonesText.gameObject.SetActive(false);
            View.UpgradeBuildingsText.gameObject.SetActive(false);
            
            if (_levelController.CurrentLevel.IsAllBuildingsMaxLevel() == false)
            {
                View.UpgradeBuildingsText.gameObject.SetActive(true);
                if (_levelController.CurrentLevel.IsAllDistrictsOpen() == false)
                {
                    View.OpenAllZonesText.gameObject.SetActive(true);
                    View.UpgradeBuildingsText.gameObject.SetActive(false);
                }
            }
            
        }

        private async void MoveToNextLevel()
        {
            _audioManager.PlaySound(TrackName.ButtonClick);
            _prefsSaveManager.PrefsData.LevelInfoModel.ClearBuildingInfo();
            _prefsSaveManager.PrefsData.LevelInfoModel.SetCurrentLevelIndex(_levelController.CurrentLevel.Level+1);
            _prefsSaveManager.ForceSave();
            await UniTask.Delay(TimeSpan.FromMilliseconds(30));
            SceneManagement.RestartCurrentScene();
        }
        
        private void BuyAccess()
        {
            _audioManager.PlaySound(TrackName.ButtonClick);
            if(_levelController.CurrentLevel.IsAllDistrictsOpen() == false) return;
            if(_levelController.CurrentLevel.IsAllBuildingsMaxLevel() == false) return;

            if (_prefsSaveManager.PrefsData.CurrenciesModel.TrySpendMoney(_cost, _levelController.CurrentLevel.Level))
            {
                _prefsSaveManager.PrefsData.LevelInfoModel.OpenAccessForLevel(_levelController.CurrentLevel.Level+1);
                View.TravelButton.gameObject.SetActive(true);
            }
        }
    }
}