namespace Gameplay.Scripts.Tutorial
{
    public abstract class TutorialStepBase
    {
        /// <summary>
        /// Initialization step logic
        /// </summary>
        /// <param name="tutorialServiceLocator">Dependencies holder</param>
        public virtual void Init(TutorialServiceLocator tutorialServiceLocator, TutorialRunner tutorialRunner){}
        
        /// <summary>
        /// Process step logic
        /// </summary>
        /// <returns>true if step done</returns>
        public abstract bool Process();
        
        /// <summary>
        /// Use for finalize after step end
        /// </summary>
        public virtual void FinalizeStep(){}
    }
}