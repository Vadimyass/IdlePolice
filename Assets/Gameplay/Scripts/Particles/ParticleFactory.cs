using System.Linq;
using UnityEngine;
using Zenject;

namespace Particles
{
    public class ParticleFactory
    {
        private ParticleConfig _particleConfig;
        private DiContainer _container;

        public ParticleFactory(DiContainer container, ParticleConfig particleConfig)
        {
            _container = container;
            _particleConfig = particleConfig;
        }

        public ParticleSystem CreateParticle(ParticleType particleType)
        {
            var reference = _particleConfig.GetParticle(particleType);
            
            if (reference.ChangebleColor == true)
            {
                var main = reference.Particle.main;
                main.startColor = reference.Color;
                reference.Particle.transform.GetComponentsInChildren<ParticleSystem>().ToList().ForEach(x =>
                {
                    var newMain = x.main;
                    newMain.startColor = reference.Color;
                });
            }

            return _container.InstantiatePrefab(reference.Particle).GetComponent<ParticleSystem>();
        }
    }
}