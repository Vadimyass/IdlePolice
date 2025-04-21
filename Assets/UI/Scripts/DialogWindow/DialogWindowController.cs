using Audio;
using BigD.Config;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts.Tutorial;
using Zenject;

namespace UI.Scripts.DialogWindow
{
    public class DialogWindowController : UIScreenController<DialogWindow>
    {
        private DialogPhrase _dialogue;
        private AudioManager _audioManager;
        private UIManager _uiManager;
        private TrackName _currentTrack;
        private LocalizationManager _localizationManager;
        private SignalBus _signalBus;
        private SpritesConfig _spritesConfig;
        private GameConfig _gameConfig;

        [Inject]
        private void Construct( UIManager uiManager, GameConfig gameConfig, SpritesConfig spritesConfig, AudioManager audioManager, LocalizationManager localizationManager, SignalBus signalBus)
        {
            _gameConfig = gameConfig;
            _spritesConfig = spritesConfig;
            _signalBus = signalBus;
            _localizationManager = localizationManager;
            _audioManager = audioManager;
            _uiManager = uiManager;
        }

        public override async UniTask Display(UIArguments arguments)
        {
            await base.Display(arguments);
            var args = (DialogWindowArguments)arguments;
            
            View.SkipStepButton.onClick.AddListener(() => _signalBus.Fire<TutorialTouch>());
            _dialogue = _gameConfig.DialogueConfig.GetDialogInitializerByDialogName(args.DialogueName);
            View.SpeakerNameText.text = _dialogue.SpeakerName;
            SetDialogPhraseInfo();
        }

        private async void SetDialogPhraseInfo()
        {
            View.SpeakerImage.sprite = _spritesConfig.GetSpriteByName(_dialogue.DialoguePicture);
            View.PhraseText.text = _dialogue.Phrase;
        }

        public override UniTask OnHide()
        {
            //View.TutorialSkipButton.onClick.RemoveAllListeners();
            //_audioManager.TurnOffSound(_currentTrack, false);
            //_vibrationManager.Vibrate(HapticTypes.LightImpact);
            return base.OnHide();
        }
    }
}