using System;
using DG.Tweening;
using Gameplay.Scripts;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Missions;
using Gameplay.Scripts.Missions.MissionsType;
using Gameplay.Scripts.Utils;
using Particles;
using TMPro;
using UI.Scripts.UpgradeWindow;
using UI.UIUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts.MainScreen
{
    public class MissionView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private TextMeshProUGUI _missionText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _claimRewardButton;
        [SerializeField] private Button _goToMissionButton;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private Image _rewardImage;
        private MissionController _missionController;
        private Mission _currentMission;
        private SpritesConfig _spritesConfig;
        private SignalBus _signalBus;
        private UnityAction _action;
        private ParticleManager _particleManager;
        private LevelController _levelController;
        private UIManager _uiManager;
        private CameraController _cameraController;

        [Inject]
        private void Construct(MissionController missionController, CameraController cameraController, UIManager uiManager, LevelController levelController, ParticleManager particleManager, SpritesConfig spritesConfig, SignalBus signalBus)
        {
            _cameraController = cameraController;
            _uiManager = uiManager;
            _levelController = levelController;
            _particleManager = particleManager;
            _signalBus = signalBus;
            _spritesConfig = spritesConfig;
            _missionController = missionController;
        }
        
        public void Init(UnityAction action)
        {
            _action = action;
            _signalBus.TryUnsubscribe<MissionCompleteSignal>(OnCompleteMission);
            _signalBus.TryUnsubscribe<MissionCheckSignal>(CheckProgress);
            _claimRewardButton.onClick.RemoveAllListeners();
            _claimRewardButton.onClick.AddListener((() =>
            {
                GetReward(_currentMission);
            }));
            _goToMissionButton.onClick.RemoveAllListeners();
            _goToMissionButton.onClick.AddListener(GoToMissionGoal);
            RefreshInfo();
            _signalBus.Subscribe<MissionCompleteSignal>(OnCompleteMission);
            _signalBus.Subscribe<MissionCheckSignal>(CheckProgress);
        }

        private void GoToMissionGoal()
        {
            switch (_currentMission)
            {
                case UpgradeObjectMission:
                    var buildingUpgrade = _levelController.CurrentLevel.GetBuildingByKey(_currentMission.MissionConfigInitializer.UpgradeBuildingValue.Key);
                    
                    buildingUpgrade.GoToUpgradeWindow();
                    break;
                case UpgradeBaseMission:
                    _uiManager.Show<UpgradeWindowController>();
                    break;
                case BuildBuildingMission:
                    var building = _levelController.CurrentLevel.GetBuildingByKey(_currentMission.MissionConfigInitializer.BuildBuildingValue.Key);
                    _cameraController.FocusOnBuilding(building.transform);
                    building.GoToBuildWindow();
                    break;
            }
        }

        private void RefreshInfo()
        {
            gameObject.SetActive(true);
            _currentMission = _missionController.GetFirstMission();
            if (_currentMission == null)
            {
                gameObject.SetActive(false);
                return;
            }
            _missionText.text = _currentMission.MissionConfigInitializer.Text;
            _iconImage.sprite = _spritesConfig.GetSpriteByName(_currentMission.MissionConfigInitializer.SpriteName);
            
            CheckProgress();
        }

        private void CheckProgress()
        {
            if(_currentMission == null) return;
            
            var progress = _currentMission.GetCurrentProgress();
            var goal =  _currentMission.GetGoal();

            _progressText.text = progress+ "/" + goal;
            _progressSlider.value = (float)progress / goal;
        }

        private async void OnCompleteMission()
        {
            if(_claimRewardButton.gameObject.activeSelf == true) return;
            
            _rewardText.text = TextMeshProUtils.NumberToShortenedText(_currentMission.MissionConfigInitializer.CurrencyValue.Value);
            _rewardImage.sprite = _currentMission.MissionConfigInitializer.CurrencyValue.Key == CurrencyUIType.Dollar
                ? _spritesConfig.GetSpriteByName(SpriteName.Money)
                : _spritesConfig.GetSpriteByName(SpriteName.Crystal);
            
            _claimRewardButton.gameObject.SetActive(true);
            await _claimRewardButton.transform.DOScale(1, 0.1f).From(0).AsyncWaitForCompletion();
            _particleManager.PlayUIParticleInPosition(ParticleType.starsSphereBurst,
                _claimRewardButton.transform.position, Quaternion.identity, 0.5f);
            _claimRewardButton.interactable = true;
        }
        
        private async void GetReward(Mission mission)
        {
            _claimRewardButton.gameObject.SetActive(false);
            
            _missionController.CompleteMission(mission);
            RefreshInfo();
            _action();
            
            switch (mission.MissionConfigInitializer.CurrencyValue.Key)
            {
                case CurrencyUIType.Dollar:
                    await _particleManager.PlayFollowParticle(CurrencyUIType.Dollar, ParticleType.moneyFollow,
                        ParticleType.moneySplashLess, transform.position);
                    break;
                case CurrencyUIType.Crystal:
                    await _particleManager.PlayFollowParticle(CurrencyUIType.Crystal, ParticleType.crystalFollow,
                        ParticleType.crystalSplashSmall, transform.position);
                    break;
                case CurrencyUIType.Donut:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _missionController.GetRewardFromMission(mission);
        }
    }
}