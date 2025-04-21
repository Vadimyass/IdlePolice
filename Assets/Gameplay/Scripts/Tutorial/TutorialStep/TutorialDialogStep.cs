using Gameplay.Scripts.Tutorial;
using Signals;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Tutorial
{
    public class TutorialDialogStep : TutorialWaiterBase, ITutorialUpdateObserver
    {
        /*private readonly DialogName _dialogName;
        private readonly System.Func<bool> _condition;
        
        private ITutorialUpdateLoop _updateLoop;
        private TutorialRunner _tutorialRunner;
        
        private bool _isButtonAvailable = true;
        private bool _isSignalAwait = true;
        private bool _isAvailableToInputSignal = false;
        private Vector2 _position;
        private FadeType _fadeType = FadeType.Normal;
        private float _width = default;
        
        private UIManager _uiManager;
        private SignalBus _signalBus;
        private int _phase;

        public override void Init(TutorialServiceLocator tutorialServiceLocator, TutorialRunner tutorialRunner)
        {
            base.Init(tutorialServiceLocator, tutorialRunner);
            _updateLoop = tutorialServiceLocator.GetService<ITutorialUpdateLoop>();
            _tutorialRunner = tutorialRunner;
            _signalBus = tutorialServiceLocator.GetService<SignalBus>();
            _uiManager = tutorialServiceLocator.GetService<UIManager>();;
        }
        
        public TutorialDialogStep(DialogName dialogName,bool isAvailableToInputSignal,int phase, System.Func<bool> condition = null)
        {
            _phase = phase;
            _isAvailableToInputSignal = isAvailableToInputSignal;
            _dialogName = dialogName;
            _condition = condition;
        }

        public TutorialDialogStep SetPosition(Vector2 position)
        {
            _position = position;
            return this;
        }

        public TutorialDialogStep SetWidth(float width)
        {
            _width = width;
            return this;
        }
        
        public TutorialDialogStep SetButtonAvailable(bool isAvailable)
        {
            _isButtonAvailable = isAvailable;
            return this;
        }

        public TutorialDialogStep SetAvailableToInputSignal()
        {
            _isAvailableToInputSignal = true;
            return this;
        }

        public TutorialDialogStep SetFadeType(FadeType fadeType)
        {
            _fadeType = fadeType;
            return this;
        }
        
        public override bool Process()
        {
            WaiterObject.SetState(TutorialWaiterState.Block, _tutorialRunner.NextStep);
            _updateLoop.AddTutorialObserver(this);
            _isSignalAwait = true;

            var argument = new DialogWindowArguments(_dialogName,_phase);
            
            _uiManager.Show<DialogWindowController>(argument);

            if (_condition == null)
            {
                _signalBus.Subscribe<UIScreenChangeStateSignal>(OnWaitSignal);
            }
            
            return true;
        }

        public bool TryUpdate()
        {
            var condition = _condition?.Invoke() ?? !_isSignalAwait;
            if (!condition) return true;
            
            EndWait();

            return false;
        }

        public override void FinalizeStep()
        {
            _signalBus.TryUnsubscribe<UIScreenChangeStateSignal>(OnWaitSignal);
            _updateLoop.RemoveTutorialObserver(this);
            _uiManager.HideLast();
            base.FinalizeStep();
        }

        private void OnWaitSignal(UIScreenChangeStateSignal obj)
        {
            if (obj.ScreenType == typeof(DialogWindow) && obj.UIScreenState == UIScreenChangeStateSignal.State.Hided)
            {
                _isSignalAwait = false;
            }
            TryUpdate();
        }

        public enum FadeType
        {
            None,
            Normal,
            Light
        }*/
        public override bool Process()
        {
            throw new System.NotImplementedException();
        }

        public bool TryUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}