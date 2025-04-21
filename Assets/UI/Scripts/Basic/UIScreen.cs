using System;
using Audio;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UIScreen : MonoBehaviour
    {
        [SerializeField] private UIAnimation _animation;
        [SerializeField] private UIType _uiType;
        public UIType UIType => _uiType;
        public UIAnimation UIAnimation => _animation;
        public bool IsShown => gameObject.activeSelf;
        
        
        [field: SerializeField] public virtual TrackName OnHideSound { get; private set; } = TrackName.Close_Window_Sound;

        [field: SerializeField] public virtual TrackName OnShowSound { get; private set; } = TrackName.Open_Window_Sound;

        private SignalBus _signalBus;
        private AudioManager _audioManager;
        
        [Inject]
        public void Construct(
            SignalBus signalBus,
            AudioManager audioManager)
        {
            _signalBus = signalBus;
            _audioManager = audioManager;
        }
        
        public async UniTask Show()
        {
            PlaySound(OnShowSound);

            gameObject.SetActive(true);
            _signalBus.Fire(new UIScreenChangeStateSignal(UIScreenChangeStateSignal.State.StartShow, GetType()));
            
            if (_animation != null && gameObject.activeSelf)
            {
                await _animation.ShowAnimation();
            }
            _signalBus.Fire(new UIScreenChangeStateSignal(UIScreenChangeStateSignal.State.Showed, GetType()));
        }
   
        public async UniTask Hide()
        {
            PlaySound(OnHideSound);

            _signalBus.Fire(new UIScreenChangeStateSignal(UIScreenChangeStateSignal.State.StartHide, GetType()));
            if (_animation != null && gameObject.activeSelf)
            {
                await _animation.HideAnimation();
            }
            
            _signalBus.Fire(new UIScreenChangeStateSignal(UIScreenChangeStateSignal.State.Hided, GetType()));
            gameObject.SetActive(false);
        }

        private void PlaySound(TrackName audio)
        {
            _audioManager.PlaySound(audio);
        }

    }
}