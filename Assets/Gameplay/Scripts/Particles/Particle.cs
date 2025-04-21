using System;
using UnityEngine;

namespace Particles
{
    [Serializable]
    public struct Particle
    {
        public ParticleType ParticleType;
        public Transform ParticlePosition;
        public Vector3 ParticleRotation;
    }
}