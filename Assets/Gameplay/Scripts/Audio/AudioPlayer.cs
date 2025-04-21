using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Audio
{
    public abstract class AudioPlayer : MonoBehaviour
    {
        public abstract void Init();
        public abstract void PlayClip(AudioClip audioClip, bool isLoop, float volume);
        public abstract void MuteClip(AudioClip audioClip, bool isMuted);

        public abstract void MuteAll(bool isMuted);
    }
}