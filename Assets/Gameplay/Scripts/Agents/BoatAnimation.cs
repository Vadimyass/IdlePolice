using DG.Tweening;
using Particles;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.Agents
{
    public class BoatAnimation : AgentAnimation
    {
        [SerializeField] private ParticleSystem _smokeParticle;
        private Sequence _moveSeq;
        private Sequence _shakeSeq;
        private ParticleManager _particleManager;
        private ParticleSystem _fightParticle;
        private Quaternion _rotation;

        [Inject]
        private void Construct(ParticleManager particleManager)
        {
            _particleManager = particleManager;
        }
        
        public override void PlayMoveAnimation()
        {
            StopMoveAnimation();
            
            _moveSeq = DOTween.Sequence();
            _moveSeq.Append(transform.DOLocalMove(Vector3.up*0.5f, 0.5f).SetEase(Ease.Linear).SetUpdate(true));
            _moveSeq.Join(transform.DOLocalRotate(-Vector3.right*6, 0.5f).SetEase(Ease.Linear).SetUpdate(true));
            _moveSeq.Append(transform.DOLocalMove(-Vector3.up*0.5f, 1f).SetEase(Ease.Linear).SetUpdate(true));
            _moveSeq.Join(transform.DOLocalRotate(Vector3.right*6, 1f).SetEase(Ease.Linear).SetUpdate(true));
            _moveSeq.Append(transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Linear).SetUpdate(true));
            _moveSeq.Join(transform.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.Linear).SetUpdate(true));
            
            _moveSeq.SetLoops(-1);
            _moveSeq.SetUpdate(true);

            if (_smokeParticle != null)
            {
                _smokeParticle.Play();
            }
        }
        
        public override void StopMoveAnimation()
        {
            if (_moveSeq != null)
            {
                _moveSeq.Kill();
            }

            if (_smokeParticle != null)
            {
                _smokeParticle.Stop();
            }
        }

        public override void PlayShakeAnimation()
        {
            _rotation = transform.rotation;
            
            StopShakeAnimation();
            _shakeSeq = DOTween.Sequence();

            _shakeSeq.Append(transform.DOScaleX(1.2f, 0.2f).SetUpdate(true));
            _shakeSeq.Join(transform.DOScaleZ(1.2f, 0.2f).SetUpdate(true));
            _shakeSeq.Join(transform.DORotate(_rotation.eulerAngles + Vector3.forward * 10 + Vector3.right * 10, 0.2f).SetUpdate(true));
            _shakeSeq.Append(transform.DOScaleX(0.8f, 0.2f).SetUpdate(true));
            _shakeSeq.Join(transform.DOScaleZ(0.8f, 0.2f).SetUpdate(true));
            _shakeSeq.Join(transform.DORotate(_rotation.eulerAngles + Vector3.forward * -10 + Vector3.right * -10, 0.2f).SetUpdate(true));
            _shakeSeq.Append(transform.DOScaleX(1.2f, 0.2f).SetUpdate(true));
            _shakeSeq.Join(transform.DOScaleZ(1.2f, 0.2f).SetUpdate(true));
            _shakeSeq.Join(transform.DORotate(_rotation.eulerAngles + Vector3.forward * -10 + Vector3.right * 10, 0.2f).SetUpdate(true));
            _shakeSeq.Append(transform.DOScaleX(0.8f, 0.2f).SetUpdate(true));
            _shakeSeq.Join(transform.DOScaleZ(0.8f, 0.2f).SetUpdate(true));
            _shakeSeq.Join(transform.DORotate(_rotation.eulerAngles + Vector3.forward * -10 + Vector3.right * 10, 0.2f).SetUpdate(true));

            _fightParticle = _particleManager.PlayParticleInPosition(ParticleType.fight, transform.position, Quaternion.identity, 10);
            
            _shakeSeq.SetLoops(-1);
            _shakeSeq.SetUpdate(true);
        }

        public override void StopShakeAnimation()
        {
            if (_fightParticle != null)
            {
                _fightParticle.Stop();
                _particleManager.ReturnParticle(_fightParticle);
            }
            
            if (_shakeSeq != null)
            {
                _shakeSeq.Kill();
            }

            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
        }
    }
}