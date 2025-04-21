using System;
using Audio;
using BigD.Config;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Utils;
using Particles;
using UI.Scripts.MainScreen;
using UI.UIUtils;
using UnityEngine;
using Zenject;

namespace UI.Scripts.MilestoneWindow
{
    public class MilestoneWindowController : UIScreenController<MilestoneWindow>
    {
        private UIManager _uiManager;
        private GameConfig _gameConfig;
        private PlayerPrefsSaveManager _prefsSaveManager;
        private LevelController _levelController;
        private SpritesConfig _spritesConfig;
        private ParticleManager _particleManager;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(UIManager uiManager, SignalBus signalBus, ParticleManager particleManager, GameConfig gameConfig, SpritesConfig spritesConfig, PlayerPrefsSaveManager prefsSaveManager, LevelController levelController)
        {
            _signalBus = signalBus;
            _particleManager = particleManager;
            _spritesConfig = spritesConfig;
            _levelController = levelController;
            _prefsSaveManager = prefsSaveManager;
            _gameConfig = gameConfig;
            _uiManager = uiManager;
        }
        
        public override async UniTask Display(UIArguments arguments)
        {
            await base.Display(arguments);
            
            View.CloseButton.onClick.RemoveAllListeners();
            View.CloseButton.onClick.AddListener((() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.HideLast();
            }));
            
            View.CollectButton.onClick.RemoveAllListeners();
            View.CollectButton.onClick.AddListener((() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                GetReward();
            }));
            
            Refresh();
        }

        private void Refresh()
        {
            
            foreach (var viewMilestoneRewardView in View.MilestoneRewardViews)
            {
                viewMilestoneRewardView.gameObject.SetActive(false);
            }

            foreach (var viewMilestoneRewardLink in View.MilestoneRewardLinks)
            {
                viewMilestoneRewardLink.gameObject.SetActive(false);
            }
            
            var currentMilestones = _gameConfig.MilestonesConfig.GetMilestonesForBase(_levelController.CurrentLevel.Level);

            for (int i = 0; i < currentMilestones.Count-1; i++)
            {
                View.MilestoneRewardLinks[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < currentMilestones.Count; i++)
            {
                View.MilestoneRewardViews[i].gameObject.SetActive(true);
                
                var isComplete = currentMilestones[i].Number <= _prefsSaveManager.PrefsData.MissionsModel.GetLastLootedMilestone(_levelController.CurrentLevel.Level);
                if (i < View.MilestoneRewardLinks.Count)
                {
                    View.MilestoneRewardLinks[i].sprite = isComplete ? View.LootedSprite : View.NotLootedSprite;
                }

                ShowMilestoneView(View.MilestoneRewardViews[i], currentMilestones[i], isComplete);
            }


            var currentMilestone = _gameConfig.MilestonesConfig.GetItemByKey(_levelController.CurrentLevel.Level,
                _prefsSaveManager.PrefsData.MissionsModel.GetLastLootedMilestone(_levelController.CurrentLevel.Level) + 1);
            
            View.CollectButton.gameObject.SetActive(
                _gameConfig.MilestonesConfig.GetFullCountOfMissionsForMilestone(_levelController.CurrentLevel.Level, currentMilestone.Number) <=
                                                    _prefsSaveManager.PrefsData.MissionsModel.GetCompletedMissionsCount(_levelController.CurrentLevel.Level));

            ShowMilestoneView(View.CurrentMilestoneRewardView, currentMilestone, false);
        }
        
        private async void GetReward()
        {
            var milestone = _gameConfig.MilestonesConfig.GetNextMilestone(_levelController.CurrentLevel.Level,
                _prefsSaveManager.PrefsData.MissionsModel.GetLastLootedMilestone(_levelController.CurrentLevel.Level));
            _prefsSaveManager.PrefsData.MissionsModel.SetLastLootedMilestone(_levelController.CurrentLevel.Level, milestone.Number);
            
            var currentMilestone = _gameConfig.MilestonesConfig.GetItemByKey(_levelController.CurrentLevel.Level,
                _prefsSaveManager.PrefsData.MissionsModel.GetLastLootedMilestone(_levelController.CurrentLevel.Level) + 1);

            if (_gameConfig.MilestonesConfig.GetFullCountOfMissionsForMilestone(_levelController.CurrentLevel.Level,
                    currentMilestone.Number) <=
                _prefsSaveManager.PrefsData.MissionsModel.GetCompletedMissionsCount(_levelController.CurrentLevel
                    .Level) == false)
            {
                _uiManager.HideLast();
            }
            else
            {
                Refresh();
            }

            switch (milestone.MilestoneReward.Key)
            {
                case MilestoneRewardType.Crystal:
                    await _particleManager.PlayFollowParticle(CurrencyUIType.Crystal, ParticleType.crystalFollow,
                        ParticleType.crystalSplashSmall, View.CurrentMilestoneRewardView.transform.position);
                    _prefsSaveManager.PrefsData.CurrenciesModel.IncreaseCrystal((int)milestone.MilestoneReward.Value);
                    break;
                case MilestoneRewardType.Basic_box:
                    _prefsSaveManager.PrefsData.ConsumablesInfoModel.AddGachaBox(GachaBoxType.Basic, 1);
                    break;
                case MilestoneRewardType.Advanced_box:
                    _prefsSaveManager.PrefsData.ConsumablesInfoModel.AddGachaBox(GachaBoxType.Advanced, 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
           
            _signalBus.Fire(new MilestoneCompleteSignal());
        }


        private void ShowMilestoneView(MilestoneRewardView milestoneRewardView, MilestoneConfigInitializer currentMilestones, bool isComplete)
        {
            Sprite sprite;
            switch (currentMilestones.MilestoneReward.Key)
            {
                case MilestoneRewardType.Crystal:
                    sprite = _spritesConfig.GetSpriteByName(SpriteName.Crystal);
                    break;
                case MilestoneRewardType.Basic_box:
                    sprite = _spritesConfig.GetSpriteByName(SpriteName.Basic_box);
                    break;
                case MilestoneRewardType.Advanced_box:
                    sprite = _spritesConfig.GetSpriteByName(SpriteName.Advanced_box);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

           
            milestoneRewardView.Show(sprite, TextMeshProUtils.NumberToShortenedText(currentMilestones.MilestoneReward.Value), isComplete);
        }
    }
}