using System.Collections.Generic;
using Gameplay.Scripts.Tutorial;
using UnityEngine;

namespace Tutorial
{
    public class TutorialArrowPointer : TutorialContextTargetFollowerObject
    {
        [SerializeField] private RectTransform _fingerRoot;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _animationName;

        private TutorialTarget _tutorialTarget;

        private Vector3 _offset;

        private readonly Dictionary<TutorialArrowPointerStep.ArrowSide, float> _angles =
            new Dictionary<TutorialArrowPointerStep.ArrowSide, float>()
            {
                {TutorialArrowPointerStep.ArrowSide.Up, 0},
                {TutorialArrowPointerStep.ArrowSide.Right, 270},
                {TutorialArrowPointerStep.ArrowSide.Down, 180},
                {TutorialArrowPointerStep.ArrowSide.Left, 90},
            };

    public void StartAnimation(TutorialTarget tutorialTarget, TutorialArrowPointerStep arrowPointerStep)
        {
            _offset = arrowPointerStep.Offset;
            
            _fingerRoot.localScale = new Vector3(arrowPointerStep.Scale, arrowPointerStep.Scale, arrowPointerStep.Scale);
            _fingerRoot.localEulerAngles = new Vector3(0, 0, _angles[arrowPointerStep.ArrowAlign]);
            
            _animator.Play(_animationName, 0, 0);
            
            StartFollowTarget(tutorialTarget);
        }
        
        protected override void UpdateByTarget(TutorialTarget target)
        {
            _fingerRoot.position = target.CalculatePosition();

            var centerPos = _fingerRoot.localPosition;
            centerPos.z = 0;

            _fingerRoot.localPosition = centerPos + _offset;
        }
    }
}