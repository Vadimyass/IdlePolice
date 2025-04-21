using System;
using System.Collections.Generic;
using Particles;
using UnityEngine;
using Zenject;

namespace Pool
{
    [Serializable]
    public class ParticlesPool : BasePool<ParticleSystem>, IDisposable
    {
        private Dictionary<ParticleType, List<ParticleSystem>> _dictionaryPool =
            new Dictionary<ParticleType, List<ParticleSystem>>();
        private ParticleFactory _particleFactory;
        
        public void Init(ParticleFactory particleFactory)
        {
            _particleFactory = particleFactory;
        }

        
        public ParticleSystem GetObject(ParticleType particleType)
        {
            _dictionaryPool.TryGetValue(particleType, out var particleList);
            if (particleList == null)
            {
                _dictionaryPool.Add(particleType, new List<ParticleSystem>());
                return CreatePoolObject(particleType);
            }

            foreach (var poolObject in particleList)
            {
                if(poolObject == null || poolObject.gameObject == null) 
                {
                    DeleteObject(particleType, poolObject);
                    return CreatePoolObject(particleType);
                }
                if (poolObject.gameObject.activeInHierarchy == false)
                {
                    poolObject.gameObject.SetActive(true);
                    return poolObject;
                }
            }

            return CreatePoolObject(particleType);
        }

        private ParticleSystem CreatePoolObject(ParticleType particleType)
        {
            var newPoolObject = _particleFactory.CreateParticle(particleType);
            _dictionaryPool[particleType].Add(newPoolObject);
            return newPoolObject;
        }
        
        public override ParticleSystem GetObject()
        {
            throw new System.NotImplementedException();
        }

        public override void ReturnObject(ParticleSystem poolObject)
        {
            poolObject.gameObject.SetActive(false);
        }

        public override void ReleaseAll()
        {
            _dictionaryPool.Clear();
        }

        public void DeleteObject(ParticleType particleType, ParticleSystem poolObject)
        {
            _dictionaryPool[particleType].Remove(poolObject);
        }

        public void Dispose()
        {
            _dictionaryPool.Clear();
            GC.SuppressFinalize(this);
        }
    }
}