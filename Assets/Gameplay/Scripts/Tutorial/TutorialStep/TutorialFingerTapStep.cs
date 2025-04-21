using System;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Tutorial;
using UnityEngine;

namespace Tutorial
{
    public class TutorialFingerTapStep : TutorialStepBase
    {
        private readonly Func<TutorialTarget, bool> _targetFinder;

        private TutorialTargetsContainer _targetsContainer;
        private TutorialFingerTap _contextFingerTap;

        public bool IsFlipX { get; private set; } = false;
        
        public bool HasIncline { get; private set; } = false;
        
        public Vector2 Offset { get; private set; } = default;
        
        public TapType StepTapType { get; private set; } = default;
        public float Scale { get; private set; } = 1f;
        
        public float Angle { get; private set; } = 0f;
        public bool IsEnableTail { get; private set; } = true;
        public bool WavesEnabled { get; private set; } = true;
        public Func<UniTask> WaitCondition { get; private set; } = null;

        public override void Init(TutorialServiceLocator tutorialServiceLocator, TutorialRunner tutorialRunner)
        {
            base.Init(tutorialServiceLocator, tutorialRunner);
            
            _targetsContainer = tutorialServiceLocator.GetService<TutorialTargetsContainer>();
            _contextFingerTap = tutorialServiceLocator.GetService<TutorialFingerTap>();
        }
        
        public TutorialFingerTapStep(Func<TutorialTarget, bool> targetFinder, TapType tapType = TapType.Tap)
        {
            _targetFinder = targetFinder;
            StepTapType = tapType;
        }

        public TutorialFingerTapStep SetTapType(TapType tapType)
        {
            StepTapType = tapType;
            return this;
        }

        public TutorialFingerTapStep FlipX()
        {
            IsFlipX = true;
            return this;
        }

        public TutorialFingerTapStep SetIncline()
        {
            HasIncline = true;
            return this;
        }

        public TutorialFingerTapStep SetWaitCondition(Func<UniTask> waitCondition)
        {
            WaitCondition = waitCondition;
            return this;
        }
        
        public TutorialFingerTapStep SetOffset(Vector2 offset)
        {
            Offset = offset;
            return this;
        }
        
        public TutorialFingerTapStep SetScale(float scale)
        {
            Scale = scale;
            return this;
        }
        
        public TutorialFingerTapStep SetAngle(float angle)
        {
            Angle = angle;
            return this;
        }

        public TutorialFingerTapStep SetEnableTail(bool isEnable)
        {
            IsEnableTail = isEnable;
            return this;
        }

        public TutorialFingerTapStep SetWavesEnabled(bool enabled)
        {
            WavesEnabled = enabled;
            return this;
        }
        
        public override bool Process()
        {
            _contextFingerTap.gameObject.SetActive(true);
            _targetsContainer.GetTarget(_targetFinder).gameObject.SetActive(true);
            _contextFingerTap.EnableTail(IsEnableTail);
            _contextFingerTap.StartAnimation(_targetsContainer.GetTarget(_targetFinder), this);
            return true;
        }

        public override void FinalizeStep()
        {
            _contextFingerTap.gameObject.SetActive(false);
            var target = _targetsContainer.GetTarget(_targetFinder);
            target.gameObject.SetActive(!target.DeactivateOnFinish);
            base.FinalizeStep();
            _contextFingerTap.StopFollow();
        }

        public enum TapType
        {
            Tap, Hold, Swipe
        }
    }
}