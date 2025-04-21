using System;
using RotaryHeart.Lib.SerializableDictionary;
using SolidUtilities.Collections;
using UnityEngine;

namespace Particles
{
    public class ParticleConfig : ScriptableObject
    {
        [SerializeField] private SerializableDictionaryBase<ParticleType, ParticleColored> _particlesDictionary =
            new SerializableDictionaryBase<ParticleType, ParticleColored>();

        public ParticleColored GetParticle(ParticleType particleType)
        {
            _particlesDictionary.TryGetValue(particleType, out var reference);
            return reference;
        }
    }

    [Serializable]
    public struct ParticleColored
    {
        public ParticleSystem Particle;
        public bool ChangebleColor;
        public Color Color;
    }
}