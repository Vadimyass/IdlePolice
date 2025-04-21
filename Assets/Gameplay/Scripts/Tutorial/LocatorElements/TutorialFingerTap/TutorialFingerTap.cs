using Gameplay.Scripts.Tutorial;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Tutorial
{
    public class TutorialFingerTap : TutorialContextTargetFollowerObject
    {
        [SerializeField] private RectTransform _fingerRoot;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _tail;
        [SerializeField] private GameObject _wavesEffect;
        
        [SerializeField] private SerializableDictionaryBase<TutorialFingerTapStep.TapType, string> _animationName;

        [field: SerializeField] public TutorialFingerTapAnimationEventsController _eventsController;
        
        private Vector3 _offset;

        public async void StartAnimation(TutorialTarget tutorialTarget, TutorialFingerTapStep tapStep)
        {
            _offset = tapStep.Offset;
            
            var inclineRotateAngle = tapStep.IsFlipX ? -40 : 40;
            _fingerRoot.localScale = new Vector3(tapStep.IsFlipX ? -tapStep.Scale : tapStep.Scale, tapStep.Scale, tapStep.Scale);
            _fingerRoot.localEulerAngles = new Vector3(0, 0, tapStep.HasIncline ? inclineRotateAngle : tapStep.Angle);
            
            _animator.Rebind();
            _animator.Update(0f);
            _animator.Play(_animationName[tapStep.StepTapType], 0, 0);
            _wavesEffect.SetActive(tapStep.WavesEnabled);

            if (tapStep.WaitCondition != null)
            {
                await tapStep.WaitCondition();
            }
            
            StartFollowTarget(tutorialTarget);
        }

        public void EnableTail(bool enabled)
        {
            _tail.SetActive(enabled);
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