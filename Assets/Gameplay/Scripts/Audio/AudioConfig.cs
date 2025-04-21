using Audio;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Installers/AudioConfig")]
    public class AudioConfig : ScriptableObject
    {
        [SerializeField] private SerializableDictionaryBase<TrackName, AudioClip> _audioReferenceData;

        public AudioClip GetAudioConfig(TrackName trackName)
        {
            _audioReferenceData.TryGetValue(trackName, out AudioClip audioClip);
            if (audioClip == null)
            {
                return null;
            }
            return audioClip;
        }
    }
}