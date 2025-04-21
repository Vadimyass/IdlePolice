using System.Linq;
using Audio;
using BigD.Config;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.Agents;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.LockedZoneManagement;
using Gameplay.Scripts.Locker;
using Managers;
using Particles;
using Tutorial;
using UI.Scripts.ChoseLevelWindow;
using UI.Scripts.OfficersWindow;
using UI.Scripts.ShopWindow;
using UI.Scripts.TravelWindow;
using UI.Scripts.UpgradeWindow;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts.MainScreen
{
    public class MainScreenController : UIScreenController<MainScreen>
    {
        private UIManager _uiManager;
        private ParticleManager _particleManager;
        private LevelController _levelController;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private SignalBus _signalBus;
        private TutorialManager _tutorialManager;
        private LockController _lockController;
        private GameConfig _gameConfig;
        private SpritesConfig _spritesConfig;

        private int CurrentZoneIndex =>
            _playerPrefsSaveManager.PrefsData.LevelInfoModel.GetZoneIndexByLevel(_levelController.CurrentLevel.Level);

        [Inject]
        private void Construct(UIManager uiManager, GameConfig gameConfig, SpritesConfig spritesConfig, LockController lockController, TutorialManager tutorialManager, SignalBus signalBus, ParticleManager particleManager, LevelController levelController, PlayerPrefsSaveManager playerPrefsSaveManager)
        {
            _spritesConfig = spritesConfig;
            _gameConfig = gameConfig;
            _lockController = lockController;
            _tutorialManager = tutorialManager;
            _signalBus = signalBus;
            _playerPrefsSaveManager = playerPrefsSaveManager;
            _levelController = levelController;
            _particleManager = particleManager;
            _uiManager = uiManager;
        }

        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            View.MilestoneSlider.Init();
            foreach (var currencyContainer in View.CurrencyContainers)
            {
                currencyContainer.Init();
                _particleManager.AddCurrencyContainer(currencyContainer.CurrencyType, currencyContainer);
            }
            
            _signalBus.Subscribe<ZoneUnlockSignal>(OnUnlockZone);
            _signalBus.Subscribe<MilestoneCompleteSignal>(TryStartTutorial);
            _signalBus.Subscribe<CreateCarSignal>(TryStartTutorial);
            _signalBus.Subscribe<ChangeCurrencySignal>(TryStartTutorial);
            _signalBus.Subscribe<OfficerGetSignal>(OnGetOfficer);
            _signalBus.Subscribe<GachaBoxGetSignal>(OnGetGachaBox);
            _signalBus.Subscribe<MainScreenTutorialStepSignal>(ShowSpecificWidgetBlock);
        }

        public override async  UniTask UpdateScreen()
        {
            await base.UpdateScreen();
            View.MilestoneSlider.Init();
        }

        private async void OnUnlockZone()
        {
            TryStartTutorial();
            if(View.ChoseLevelButton.gameObject.activeSelf == true) return;
            
            View.ChoseLevelButton.interactable = false;
            var pos = View.ChoseLevelButton.transform.localPosition;
            View.ChoseLevelButton.gameObject.SetActive(CurrentZoneIndex >= 1);
            View.ChoseLevelButton.transform.localPosition = Vector3.zero;
            await View.ChoseLevelButton.transform.DOScale(1.5f, 0.2f).From(0.75f).AsyncWaitForCompletion();
            View.ChoseLevelButton.transform.DOScale(1f, 0.1f);
            View.ChoseLevelButton.transform.DOLocalMoveX(pos.x, 0.5f).SetEase(Ease.OutCirc);
            await View.ChoseLevelButton.transform.DOLocalMoveY(pos.y, 0.5f).SetEase(Ease.InCirc).AsyncWaitForCompletion();
            
            View.ChoseLevelButton.interactable = true;
        }
        
        
        public override async UniTask Display(UIArguments arguments)
        {
            await base.Display(arguments);
            View.UpgradesButton.onClick.RemoveAllListeners();
            View.OfficersButton.onClick.RemoveAllListeners();
            View.ChoseLevelButton.onClick.RemoveAllListeners();
            View.OpenShopButton.onClick.RemoveAllListeners();
            
            _audioManager.PlayMusic(TrackName.MainMusic, 0.45f);

            TryStartTutorial();


            foreach (var transformsList in View.HideBeforeTutor)
            {
                if (_playerPrefsSaveManager.PrefsData.TutorialModel.IsContextTutorDone(transformsList.Key) == false)
                {
                    foreach (var transform in transformsList.Value.List)
                    {
                        transform.alpha = 0;
                        transform.blocksRaycasts = false;
                    }
                }
                else
                {
                    foreach (var transform in transformsList.Value.List)
                    {
                        transform.alpha = 1;
                        transform.blocksRaycasts = true;
                    }
                }
            }
            
            View.UpgradesButton.onClick.AddListener((() =>
            {
                if(_lockController.HaveLock<LootMissionLocker>() || _lockController.HaveLock<BuildingLocker>() || _lockController.HaveLock<OpenDistrictLocker>() || _lockController.HaveLock<ShopLocker>() == true) return;
                
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.Show<UpgradeWindowController>();
            }));
            View.OfficersButton.onClick.AddListener((() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.Show<OfficersWindowController>();
            }));
            View.ChoseLevelButton.onClick.AddListener((() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.Show<TravelWindowController>();
            }));
            View.OpenShopButton.onClick.AddListener((() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.Show<ShopWindowController>();
            }));
            
            View.ChoseLevelButton.gameObject.SetActive(CurrentZoneIndex >= 1);
            
            if (_tutorialManager.TryStartTutorial(TutorialType.WelcomeTutorial))
            {
                foreach (var transform in View.WidgetBlocks)
                {
                    transform.Hide();
                }
                return;
            }
        }

        private async void OnGetOfficer(OfficerGetSignal signal)
        {
            var sprite = _spritesConfig.GetSpriteByName(_gameConfig.OfficersInfoConfig.GetItemByKey(signal.Key).SpriteName);

            var card = View.SmallOfficerCardPool.GetObject();
            card.Show(sprite);

            var vector = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), 0);
            var seq = DOTween.Sequence();
            seq.Append(card.transform.DOLocalMove(card.transform.localPosition + vector*300, 0.75f).SetEase(Ease.OutCirc));
            seq.Append(card.transform.DOMove(View.OfficersButton.transform.position, 1.25f)
                .SetEase(Ease.OutCirc));
            seq.Append(card.transform.DOScale(0, 0.25f));
            
            await seq.AsyncWaitForCompletion();
            
            View.SmallOfficerCardPool.ReturnObject(card);
        }
        
        private async void OnGetGachaBox(GachaBoxGetSignal signal)
        {
            var sprite = _spritesConfig.GetGachaBoxSpriteByType(signal.GachaBoxType);

            var card = View.GachaBoxSmallPool.GetObject();
            card.sprite = sprite;

            var vector = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), 0);
            var seq = DOTween.Sequence();
            seq.Append(card.transform.DOLocalMove(card.transform.localPosition + vector*300, 0.75f).SetEase(Ease.OutCirc));
            seq.Append(card.transform.DOMove(View.OpenShopButton.transform.position, 1.25f)
                .SetEase(Ease.OutCirc));
            seq.Append(card.transform.DOScale(0, 0.25f));
            
            await seq.AsyncWaitForCompletion();
            
            View.GachaBoxSmallPool.ReturnObject(card);
        }
        
        private void TryStartTutorial()
        {
            
            if (_levelController.CurrentLevel.GetCarCount() >= 3 &&
                _playerPrefsSaveManager.PrefsData.TutorialModel.IsContextTutorDone(TutorialType.UpgradeTutorial) == false)
            {
                _tutorialManager.TryStartTutorial(TutorialType.UpgradeTutorial);
            }
            else if (_levelController.CurrentLevel.GetCarCount() >= 4 &&
                     _playerPrefsSaveManager.PrefsData.TutorialModel.IsContextTutorDone(TutorialType.OpenDistrict) == false &&
                     _playerPrefsSaveManager.PrefsData.CurrenciesModel.BasesMoney[_levelController.CurrentLevel.Level] >= 3000)
            {
                _tutorialManager.TryStartTutorial(TutorialType.OpenDistrict);
            }else if (_playerPrefsSaveManager.PrefsData.ConsumablesInfoModel.GachaBoxes.Any(x => x.Value > 0))
            {
                _tutorialManager.TryStartTutorial(TutorialType.ShopTutorial);
            }
            
            if (_levelController.CurrentLevel.IsAllDistrictsOpen())
            {
                _tutorialManager.TryStartTutorial(TutorialType.NewBaseTutorial);
            }
            
        }
        
        private void ShowSpecificWidgetBlock(MainScreenTutorialStepSignal mainScreenTutorialStepSignal)
        {
            var widgetBlock = View.WidgetBlocks.FirstOrDefault(x =>
                x.widgetBlockType == mainScreenTutorialStepSignal.MainScreenWidgetBlockType);
            
            if (widgetBlock == null)
            {
                return;
            }

            widgetBlock.StartBlowing(mainScreenTutorialStepSignal.IsOnce);
        }
        public override async UniTask OnHide()
        {
            View.UpgradesButton.onClick.RemoveAllListeners();
            View.OfficersButton.onClick.RemoveAllListeners();
            View.ChoseLevelButton.onClick.RemoveAllListeners();
            await base.OnHide();
        }
    }
}