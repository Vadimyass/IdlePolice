using System;
using System.Collections.Generic;
using BigD.Config;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Gameplay.Scripts.Missions;
using Gameplay.Scripts.Utils;
using Particles;
using UI.Scripts.MilestoneWindow;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts.MainScreen
{
    public class MilestoneSlider : MonoBehaviour
    {
        [SerializeField] private MissionView _missionView;
        [SerializeField] private Slider _slider;
        [SerializeField] private List<Transform> _missionsTransforms;
        [SerializeField] private List<Image> _missionsImages;
        [SerializeField] private Button _lootButton;
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _unactiveSprite;
        
        private GameConfig _gameConfig;
        private LevelController _levelController;
        private PlayerPrefsSaveManager _prefsSaveManager;
        private SignalBus _signalBus;
        private MilestoneConfigInitializer _milestone;
        private ParticleManager _particleManager;
        private UIManager _uiManager;

        [Inject]
        private void Construct(GameConfig gameConfig, ParticleManager particleManager, UIManager uiManager, LevelController levelController, PlayerPrefsSaveManager prefsSaveManager, SignalBus signalBus)
        {
            _uiManager = uiManager;
            _particleManager = particleManager;
            _signalBus = signalBus;
            _prefsSaveManager = prefsSaveManager;
            _levelController = levelController;
            _gameConfig = gameConfig;
            _signalBus.Subscribe<MilestoneCompleteSignal>(SetMilestone);
        }
        
        public void Init()
        {
            _missionView.Init(OnMissionComplete);
            SetMilestone();
            _lootButton.onClick.AddListener(()=>
            {
                _uiManager.Show<MilestoneWindowController>();
            });
        }


        private async void OnMissionComplete()
        {
            var currentProgress =
                _prefsSaveManager.PrefsData.MissionsModel.GetCompletedMissionsCount(_levelController.CurrentLevel
                    .Level);
            var fullCountForMilestone =
                _gameConfig.MilestonesConfig.GetFullCountOfMissionsForMilestone(_levelController.CurrentLevel.Level,
                    _milestone.Number);

            await _slider.DOValue(currentProgress - fullCountForMilestone + _milestone.MissionsTarget, 1).AsyncWaitForCompletion();

            _missionsImages[(int)_slider.value-1].sprite = _activeSprite;

            if (_slider.value >= _slider.maxValue)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(2));
                _uiManager.Show<MilestoneWindowController>();
            }
        }

        private void SetMilestone()
        {
            _milestone = _gameConfig.MilestonesConfig.GetNextMilestone(_levelController.CurrentLevel.Level,
                _prefsSaveManager.PrefsData.MissionsModel.GetLastLootedMilestone(_levelController.CurrentLevel.Level));

            _slider.minValue = 0;
            _slider.maxValue = _milestone.MissionsTarget;
            
            var currentProgress =
                _prefsSaveManager.PrefsData.MissionsModel.GetCompletedMissionsCount(_levelController.CurrentLevel
                    .Level);
            var fullCountForMilestone =
                _gameConfig.MilestonesConfig.GetFullCountOfMissionsForMilestone(_levelController.CurrentLevel.Level,
                    _milestone.Number);
             _slider.value = currentProgress - fullCountForMilestone + _milestone.MissionsTarget;

             if (_slider.value >= _slider.maxValue)
            {
                _lootButton.interactable = true;
            }
            
            foreach (var missionsTransform in _missionsTransforms)
            {
                missionsTransform.gameObject.SetActive(false);
            }

            for (int i = 0; i < _milestone.MissionsTarget-1; i++)
            {
                _missionsTransforms[i].gameObject.SetActive(true);
                _missionsImages[i].sprite = _unactiveSprite;
            }

            for (int i = 0; i < _slider.value; i++)
            {
                _missionsImages[i].sprite = _activeSprite;
            }
        }
    }
}