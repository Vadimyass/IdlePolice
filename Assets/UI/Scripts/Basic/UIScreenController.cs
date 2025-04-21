using Audio;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public abstract class UIScreenController<T> : UIScreenController where T : UIScreen
    {
        protected T View => screenView;
        private T screenView;
        protected AudioManager _audioManager;
        
        public override UIScreen UIScreen => screenView;
        [Inject]
        private void Construct(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        
        public override void Init(UIScreen uiScreen)
        {
            screenView = uiScreen as T;
            _uiType = View.UIType;
        }

        public override async UniTask Display(UIArguments arguments)
        {
            await screenView.Show();
        }

        public override async  UniTask UpdateScreen()
        {
            await screenView.Show();
        }

        public override async UniTask OnHide()
        {
            _audioManager.PlaySound(TrackName.Close_Window_Sound);
            await View.Hide();
        }
        
    }
    
    public abstract class UIScreenController
    {
        public abstract UIScreen UIScreen { get; }
        public abstract void Init(UIScreen uiScreen);
        public abstract UniTask Display(UIArguments arguments);
        public abstract UniTask UpdateScreen();
        public abstract UniTask OnHide();
        protected UIType _uiType;
        public UIType UIType => _uiType;
    }
}