using Cysharp.Threading.Tasks;
using Particles;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public abstract class UIAnimation : MonoBehaviour
    {
        [SerializeField] protected float _animationDuration;
        [SerializeField] protected RectTransform _panelTransform;
        [SerializeField] protected Image _backgroundImage;
        protected ParticleManager _particleManager;

        [Inject]
        private void Construct(ParticleManager particleManager)
        {
            _particleManager = particleManager;
        }
        
        public abstract UniTask ShowAnimation();

        public abstract UniTask HideAnimation();
    }
}