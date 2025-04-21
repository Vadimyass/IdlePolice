using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.DataProfiling;
using Pool;
using UI;
using UI.Scripts.MainScreen;
using UniRx;
using UnityEngine;
using Zenject;

namespace Particles
{
    public class ParticleManager : MonoBehaviour, IDisposable
    {
        [SerializeField] private ParticlesPool _particlesPool;
        private ParticleFactory _particleFactory;
        private CompositeDisposable _disposables = new CompositeDisposable();
        private Dictionary<CurrencyUIType,CurrencyContainer> _currencyContainers = new ();
        private Dictionary<CurrencyUIType,CurrencyContainer> _currencyContainersSecond = new ();
        private Dictionary<ParticleType,Vector3> _particleScales = new ();
        private UIManager _uiManager;

        [Inject]
        private void Construct(ParticleFactory particleFactory, UIManager uiManager)
        {
            _uiManager = uiManager;
            _particleFactory = particleFactory;
        }

        private void Start()
        {
            _particlesPool.Init(_particleFactory);
        }

        public void AddCurrencyContainer(CurrencyUIType currencyType, CurrencyContainer coinContainer)
        {
            _currencyContainers.TryAdd(currencyType, coinContainer);
        }
        
        public void AddCurrencyContainerForShop(CurrencyUIType currencyType, CurrencyContainer coinContainer)
        {
            _currencyContainersSecond.TryAdd(currencyType, coinContainer);
        }
        
        public ParticleSystem PlayParticleInTransform(ParticleType particleType, Transform parent, Quaternion rotation)
        {
            var particle = _particlesPool.GetObject(particleType);
            particle.transform.parent = parent;
            particle.transform.position = parent.position;
            particle.transform.localRotation = rotation;
            particle.transform.localScale = Vector3.one;

            PlayParticle(particle);
            return particle;
        }
        
        public ParticleSystem PlayParticleInTransformWithOffset(ParticleType particleType, Transform parent, Quaternion rotation,Vector3 offset,float scale = 1)
        {
            var particle = _particlesPool.GetObject(particleType);
            particle.transform.parent = parent;
            particle.transform.position = parent.position + offset;
            particle.transform.localRotation = rotation;
            particle.transform.localScale = Vector3.one * scale;

            PlayParticle(particle);
            return particle;
        }
        
        public ParticleSystem PlayUIParticleInPosition(ParticleType particleType, Vector3 position, Quaternion rotation, float scale = 1)
        {
            var particle = _particlesPool.GetObject(particleType);
            particle.transform.parent = _uiManager.transform;
            particle.transform.position = position;
            particle.transform.localRotation = rotation;

            var particleScale = particle.transform.localScale;
            if (_particleScales.ContainsKey(particleType) == true)
            {
                _particleScales.TryGetValue(particleType, out particleScale);
            }
            else
            {
                _particleScales.Add(particleType, particleScale);
            }
            
            particle.transform.localScale = particleScale * scale;

            PlayParticle(particle);
            return particle;
        }
        
        public ParticleSystem PlayUIParticleInTransform(ParticleType particleType, Transform parent, Quaternion rotation, float scale = 1)
        {
            var particle = _particlesPool.GetObject(particleType);
            particle.transform.parent = parent;
            particle.transform.position = parent.position;
            particle.transform.localRotation = rotation;
            
            var particleScale = particle.transform.localScale;
            if (_particleScales.ContainsKey(particleType) == true)
            {
                _particleScales.TryGetValue(particleType, out particleScale);
            }
            else
            {
                _particleScales.Add(particleType, particleScale);
            }
            
            particle.transform.localScale = particleScale * scale;

            PlayParticle(particle);
            return particle;
        }
        
        public ParticleSystem PlayParticleInPosition(ParticleType particleType, Vector3 position, Quaternion rotation, float scale = 1)
        {
            var particle = _particlesPool.GetObject(particleType);
            particle.transform.position = position;
            particle.transform.localRotation = rotation;
            particle.transform.localScale = Vector3.one * scale;
            
            PlayParticle(particle);
            return particle;
        }

        private void PlayParticle(ParticleSystem particle)
        {
            particle.Play();
            
            if (particle.main.loop == false)
            {
                int timeWait = (int)Math.Ceiling(particle.main.duration);
                Observable.Timer(TimeSpan.FromSeconds(timeWait)).Subscribe(_ =>
                {
                    ReturnParticle(particle);
                }).AddTo(_disposables);
            }
        }

        public async void ReturnParticle(ParticleSystem particle)
        {
            if(particle == null || particle.gameObject == null) return;
            _particlesPool.ReturnObject(particle);
            particle.transform.parent = transform;
        }

        public void Dispose()
        {
            _disposables.Clear();
            _particlesPool.Dispose();
        }

        public async UniTask PlayFollowParticle(CurrencyUIType currencyType, ParticleType followParticle, ParticleType endParticle, Vector3 startPos, bool isMainScreen = true, float scale = 1)
        {
            bool isGet = false;
            CurrencyContainer target = null;
            if (isMainScreen == true)
            {
                isGet = _currencyContainers.TryGetValue(currencyType, out target);
            }
            else
            {
                isGet = _currencyContainersSecond.TryGetValue(currencyType, out target);
            }

            if(isGet == false || target == null) return;
            
            var particle = _particlesPool.GetObject(followParticle);
            particle.transform.parent = _uiManager.transform;
            particle.transform.position = startPos;
            var particleScale = particle.transform.localScale;
            particle.transform.localScale *= scale;
            PlayParticle(particle);
            
            var seq = DOTween.Sequence();
            seq.Append(particle.transform.DOScale(particle.transform.localScale / 3, 0.5f).SetEase(Ease.Linear));
            seq.Join(particle.transform.DOMove(target.Image.transform.position, 0.5f));
            await seq.AsyncWaitForCompletion();
            
            particle.transform.localScale = particleScale;
            
            PlayUIParticleInPosition(endParticle, target.Image.transform.position, Quaternion.identity);
        }
    }
}