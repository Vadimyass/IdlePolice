using Audio;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.DataProfiling.PrefsData;
using UnityEngine;
using Zenject;

namespace UI.Scripts.ExitWindow
{
    public class ExitWindowController : UIScreenController<ExitWindow>
    {
        private UIManager _uiManager;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;

        [Inject]
        private void Construct(UIManager uiManager,PlayerPrefsSaveManager playerPrefsSaveManager)
        {
            _playerPrefsSaveManager = playerPrefsSaveManager;
            _uiManager = uiManager;
        }

        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            View.AcceptButton.onClick.AddListener(Accept);
            View.DeclineButton.onClick.AddListener(Decline);
        }

        public override async UniTask Display(UIArguments arguments)
        {
            _playerPrefsSaveManager.ForceSave();
            await base.Display(arguments);
        }
        

        private void Accept()
        {
            _audioManager.PlaySound(TrackName.ButtonClick);
            Application.Quit();
        }

        private void Decline()
        {
            _audioManager.PlaySound(TrackName.ButtonClick);
            _uiManager.HideLast();
        }
    }
}