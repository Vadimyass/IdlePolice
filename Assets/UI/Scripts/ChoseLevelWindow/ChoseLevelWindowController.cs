using Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.DataProfiling.PrefsData;
using UnityEngine;
using Zenject;

namespace UI.Scripts.ChoseLevelWindow
{
    public class ChoseLevelWindowController : UIScreenController<ChoseLevelWindow>
    {
        private PlayerPrefsSaveManager _prefsSaveManager;
        private UIManager _uiManager;

        [Inject]
        private void Construct(PlayerPrefsSaveManager prefsSaveManager, UIManager uiManager)
        {
            _uiManager = uiManager;
            _prefsSaveManager = prefsSaveManager;
        }

        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            StartAnimationBackground();
        }

        public override async UniTask Display(UIArguments arguments)
        {
            await base.Display(arguments);
            for (int i = 0; i < View.LevelViews.Count; i++)
            {
                View.LevelViews[i].Init(i);
            }
            
            var currentProgress = _prefsSaveManager.PrefsData.LevelInfoModel.CurrentLevelIndex;

            for (int i = 0; i < currentProgress-1; i++)
            {
                View.LevelViews[i].ShowAsComplete();
                View.LevelLinks[i].sprite = View.ActiveSpriteLink;
            }
            
            View.LevelViews[currentProgress].ShowAsCurrent();

            for (int i = currentProgress+1; i < View.LevelViews.Count; i++)
            {
                View.LevelViews[i].ShowAsLocked();
                View.LevelLinks[i-1].sprite = View.UnactiveSpriteLink;
            }
            
            View.CloseButton.onClick.RemoveAllListeners();
            View.CloseButton.onClick.AddListener((() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.HideLast();
            }));
        }

        public override UniTask OnHide()
        {
            View.CloseButton.onClick.RemoveAllListeners();
            return base.OnHide();
        }

        private void StartAnimationBackground()
        {
            View.BackgroundTransform.DOLocalMove(new Vector3(64.2f, -184.83f, 0), 3).From(new Vector3(-556.43f, 435.51f, 0)).SetLoops(-1).SetEase(Ease.Linear);
        }
    }
}