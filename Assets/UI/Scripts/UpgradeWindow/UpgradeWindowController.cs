using System.Collections.Generic;
using System.Linq;
using Audio;
using BigD.Config;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Locker;
using Gameplay.Scripts.Utils;
using Zenject;

namespace UI.Scripts.UpgradeWindow
{
    public class UpgradeWindowController : UIScreenController<UpgradeWindow>
    {
        private List<UpgradeView> _upgradeViews = new List<UpgradeView>();
        private GameConfig _gameConfig;
        private PlayerPrefsSaveManager _prefsSaveManager;
        private SignalBus _signalBus;
        private UIManager _uiManager;
        private BaseUpgradesController _baseUpgradesController;
        private SpritesConfig _spritesConfig;
        private LevelController _levelController;
        private LockController _lockController;

        [Inject]
        private void Construct(GameConfig gameConfig, LockController lockController, UIManager uiManager, SpritesConfig spritesConfig, LevelController levelController, BaseUpgradesController baseUpgradesController, PlayerPrefsSaveManager prefsSaveManager, SignalBus signalBus)
        {
            _lockController = lockController;
            _levelController = levelController;
            _spritesConfig = spritesConfig;
            _baseUpgradesController = baseUpgradesController;
            _uiManager = uiManager;
            _signalBus = signalBus;
            _prefsSaveManager = prefsSaveManager;
            _gameConfig = gameConfig;
            _signalBus.Subscribe<BaseUpgradeSignal>(OnBaseUpgrade);
        }

        private void OnBaseUpgrade(BaseUpgradeSignal obj)
        {
            var view = _upgradeViews.First(x => x.BaseUpgradeConfigInitializator.Key == obj.Key);
            if (view != null)
            {
                View.UpgradeViewPool.ReturnObject(view);
            }
        }

        public override async UniTask Display(UIArguments arguments)
        {
            await base.Display(arguments);

            View.UpgradeViewPool.ReturnAll();
            View.CloseButton.onClick.AddListener((() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.HideLast();
            }));
            
            foreach (var item in  _gameConfig.UpgradeBaseConfig.Items[0])
            {
                if (item.DistrictNeed-1 <= _prefsSaveManager.PrefsData.LevelInfoModel.GetZoneIndexByLevel(_levelController.CurrentLevel.Level))
                {
                    if (_prefsSaveManager.PrefsData.BaseUpgradesModel.CheckIsBought(item.Key, _levelController.CurrentLevel.Level) == false)
                    {
                        var view = View.UpgradeViewPool.GetObject();
                        view.Init(_prefsSaveManager, _lockController, _audioManager, _signalBus, _levelController, _baseUpgradesController, _spritesConfig);
                        view.Show(item);
                        _upgradeViews.Add(view);
                    }
                }
            }
           
        }
    }
}