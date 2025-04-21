using System;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Utils;
using TMPro;
using UI.UIUtils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts.ChoseLevelWindow
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private Button _selfButton;
        [SerializeField] private Image _circleImage;
        [SerializeField] private Transform _completeTransform;
        [SerializeField] private Transform _currentTransform;
        [SerializeField] private Transform _lockedTransform;
        private int _levelIndex;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;

        [Inject]
        private void Construct(PlayerPrefsSaveManager playerPrefsSaveManager)
        {
            _playerPrefsSaveManager = playerPrefsSaveManager;
        }
        
        public void Init(int index)
        {
            _levelIndex = index;
            _selfButton.onClick.AddListener(OnClick);
        }

        private async void OnClick()
        {
            _playerPrefsSaveManager.PrefsData.LevelInfoModel.SetCurrentLevelIndex(_levelIndex);
            _playerPrefsSaveManager.ForceSave();
            await UniTask.Delay(TimeSpan.FromMilliseconds(30));
            SceneManagement.LoadBootScene();
        }

        public void ShowAsComplete()
        {
            _circleImage.color = ImageUtils.GetColorByIntRGB(73, 93, 139);
            _completeTransform.gameObject.SetActive(true);
            _currentTransform.gameObject.SetActive(false);
            _lockedTransform.gameObject.SetActive(false);
        }

        public void ShowAsCurrent()
        {
            _circleImage.color = ImageUtils.GetColorByIntRGB(146, 211, 65);
            _completeTransform.gameObject.SetActive(false);
            _currentTransform.gameObject.SetActive(true);
            _lockedTransform.gameObject.SetActive(false);
        }

        public void ShowAsLocked()
        {
            _circleImage.color = ImageUtils.GetColorByIntRGB(73, 93, 139);
            _completeTransform.gameObject.SetActive(false);
            _currentTransform.gameObject.SetActive(false);
            _lockedTransform.gameObject.SetActive(true);
        }
    }
}