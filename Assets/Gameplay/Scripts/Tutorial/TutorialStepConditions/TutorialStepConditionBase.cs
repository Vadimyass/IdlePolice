using Managers;
using Zenject;

namespace Tutorial
{
    public class TutorialStepConditionBase
    {
        protected SignalBus _signalBus;
        protected TutorialManager _tutorialManager;
        
        [Inject]
        private void Construct(SignalBus signalBus, TutorialManager tutorialManager)
        {
            _signalBus = signalBus;
            _tutorialManager = tutorialManager;
        }

        public virtual void Setup()
        {
            
        }

        public virtual void Dispose()
        {
            
        }
    }
}