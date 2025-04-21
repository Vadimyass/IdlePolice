using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Scripts.Utils;

namespace Gameplay.Scripts.Tutorial
{
    public class TutorialRunner
    {
         public int StepsCount => _steps?.Count ?? 0;

        private TutorialWaiterObject _waiterObject;

        public bool IsActiveTutorial => _isActiveTutorial;
        private bool _isActiveTutorial = false;
        private int _stepIndex = 0;
        private List<TutorialStepBase> _steps;
        private TutorialStepBase _prevStep;

        public void Configurate()
        {
            _waiterObject = new TutorialWaiterObject();
        }

        public void SetTutorial(ITutorial tutorial, TutorialServiceLocator tutorialServiceLocator)
        {
            if (tutorial == null)
            {
                return;
            }
            
            _isActiveTutorial = true;
            BigDDebugger.LogError("Runner", _isActiveTutorial);
            
            tutorialServiceLocator.SetService(_waiterObject);

            tutorial.Configurate();

            var install = tutorial as IInstall;
            install?.Install(tutorialServiceLocator);

            _stepIndex = 0;
            _steps = tutorial.GetSteps();

            foreach (var step in _steps)
            {
                step.Init(tutorialServiceLocator, this);
            }
        }

        public void NextStep()
        {
            if (_stepIndex >= StepsCount)
            {
                Clear();
                return;
            }

            if (_waiterObject.WaiterState == TutorialWaiterState.Block)
            {
                return;
            }

            if (_prevStep != null && _prevStep != _steps[_stepIndex])
            {
                _prevStep.FinalizeStep();
            }
            
            _prevStep = _steps[_stepIndex];

            if (_prevStep.Process())
            {
                _stepIndex++;
            }

            if (_waiterObject.WaiterState == TutorialWaiterState.StartNextInstant)
            {
                _waiterObject.SetState(TutorialWaiterState.None);
                NextStep();
                return;
            }

            if (_waiterObject.WaiterState == TutorialWaiterState.EndInstant)
            {
                Clear();
                return;
            }
        }
        
        public void Clear()
        {
            _waiterObject.Clear();
            _prevStep?.FinalizeStep();
            _prevStep = null;
            _stepIndex = 0;
            _isActiveTutorial = false;
        }
    }
}