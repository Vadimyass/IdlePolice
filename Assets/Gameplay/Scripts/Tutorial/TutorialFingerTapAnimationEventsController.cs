using UniRx;
using UnityEngine;

namespace Gameplay.Scripts.Tutorial
{
    public class TutorialFingerTapAnimationEventsController
    {
        [SerializeField] private ParticleSystem _roundParticle;
        
        public BoolReactiveProperty IsSpinBtnHold { get; private set; }

        private void Awake()
        {
            IsSpinBtnHold = new BoolReactiveProperty(false);
        }

        public void EmitRoundParticle() 
        {
            _roundParticle.Emit(1);       
        }

        public void OnStartSpinButtonHold()
        {
            IsSpinBtnHold.Value = true;
        }

        public void OnStopSpinButtonHold()
        {
            IsSpinBtnHold.Value = false;
        }
    }
}