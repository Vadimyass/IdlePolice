using System.Collections.Generic;
using Zenject;

namespace Tutorial
{
    public class TutorialStepConditionController
    {
        //private readonly List<TutorialStepConditionBase> _tutorialStepCondition = new List<TutorialStepConditionBase>();
        
        [Inject]
        private void Construct(DiContainer diContainer)
        {
            // _tutorialStepCondition.Add(new LocationPageOnboardingTutorialStepCondition());
            // _tutorialStepCondition.Add(new SlotMachinePageOnboardingTutorialStepCondition());
            // _tutorialStepCondition.Add(new SlotMachineSuperBetTutorialStepCondition());

            // foreach (var conditionBase in _tutorialStepCondition)
            // {
            //     diContainer.Inject(conditionBase);
            //     conditionBase.Setup();
            // }
        }
    }
}

