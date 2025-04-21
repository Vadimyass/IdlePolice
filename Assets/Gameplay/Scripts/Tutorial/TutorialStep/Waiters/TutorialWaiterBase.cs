using Gameplay.Scripts.Tutorial;

namespace Tutorial
{
    public abstract class TutorialWaiterBase : TutorialStepBase
    {
        protected TutorialWaiterObject WaiterObject { get; private set; }
        
        public override void Init(TutorialServiceLocator tutorialServiceLocator, TutorialRunner tutorialRunner)
        {
            base.Init(tutorialServiceLocator, tutorialRunner);
            WaiterObject = tutorialServiceLocator.GetService<TutorialWaiterObject>();
        }

        /// <summary>
        /// Stop waiting logic
        /// </summary>
        protected virtual void EndWait()
        {
            WaiterObject.SetState(TutorialWaiterState.None);
        }
    }
}