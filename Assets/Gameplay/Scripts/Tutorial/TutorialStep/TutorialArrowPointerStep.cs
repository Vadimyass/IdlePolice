using System;
using Gameplay.Scripts.Tutorial;
using UnityEngine;

namespace Tutorial
{
    public class TutorialArrowPointerStep : TutorialStepBase
    {
        private readonly Func<TutorialTarget, bool> _targetFinder;

        private TutorialTargetsContainer _targetsContainer;
        private TutorialArrowPointer _contextArrowPointer;

        public Vector2 Offset { get; private set; } = default;
        
        public ArrowSide ArrowAlign { get; private set; } = default;
        public float Scale { get; private set; } = 1f;

        public override void Init(TutorialServiceLocator tutorialServiceLocator, TutorialRunner tutorialRunner)
        {
            base.Init(tutorialServiceLocator, tutorialRunner);
            
            _targetsContainer = tutorialServiceLocator.GetService<TutorialTargetsContainer>();
            _contextArrowPointer = tutorialServiceLocator.GetService<TutorialArrowPointer>();
        }
        
        public TutorialArrowPointerStep(Func<TutorialTarget, bool> targetFinder, ArrowSide arrowAlign = ArrowSide.Up)
        {
            _targetFinder = targetFinder;
            ArrowAlign = arrowAlign;
        }

        public TutorialArrowPointerStep SetArrowAlign(ArrowSide arrowAlign)
        {
            ArrowAlign = arrowAlign;
            return this;
        }

        public TutorialArrowPointerStep SetOffset(Vector2 offset)
        {
            Offset = offset;
            return this;
        }
        
        public TutorialArrowPointerStep SetScale(float scale)
        {
            Scale = scale;
            return this;
        }

        public override bool Process()
        {
            _contextArrowPointer.gameObject.SetActive(true);
            _contextArrowPointer.StartAnimation(_targetsContainer.GetTarget(_targetFinder), this);
            return true;
        }

        public override void FinalizeStep()
        {
            _contextArrowPointer.gameObject.SetActive(false);
            base.FinalizeStep();
            _contextArrowPointer.StopFollow();
        }

        public enum ArrowSide
        {
            Up, Down, Left, Right
        }
    }
}