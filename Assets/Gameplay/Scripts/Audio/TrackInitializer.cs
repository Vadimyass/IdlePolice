using System;
using Audio;
using MyBox;
using MyBox.Internal;

namespace Gameplay.Scripts.Audio
{
    [Serializable]
    public class TrackInitializer
    {
        public bool IsPlaySound;
        [ConditionalField(nameof(IsPlaySound)), SearchableEnum] public TrackName TrackName;
        [ConditionalField(nameof(IsPlaySound))] public bool IsLoop;
        [ConditionalField(nameof(IsPlaySound))] public bool IsWorldSound = true;
    }
}