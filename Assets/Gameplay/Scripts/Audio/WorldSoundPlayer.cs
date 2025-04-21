using System;
using Audio;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Pool;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.Audio
{
    public class WorldSoundPlayer : AudioPlayer
    {
        [SerializeField] private AudioSourcePool _audioSourcePool;
        private bool _isLoop;
        private PlayerPrefsSaveManager _playerPrefsData;
        private bool IsMuted => _playerPrefsData == null || !_playerPrefsData.PrefsData.SettingsModel.IsSettingEnabled(SettingType.Sound);

        [Inject]
        private void Construct(PlayerPrefsSaveManager playerPrefsData)
        {
            _playerPrefsData = playerPrefsData;
            _audioSourcePool.CreateAudioSources(10);
        }

        public AudioSource PlayClip(AudioClip audioClip, bool isLoop, Transform parent)
        {
            if (IsMuted) return null;

            var audioSource = _audioSourcePool.GetObject();
            audioSource.transform.parent = parent;
            audioSource.transform.localPosition = Vector3.zero;
            audioSource.mute = false;
            audioSource.clip = audioClip;
            audioSource.loop = isLoop;
            audioSource.Play();

            if (audioSource.loop == false)
            {
                int timeWait = (int)Math.Ceiling(audioClip.length);
                Observable.Timer(TimeSpan.FromSeconds(timeWait)).Subscribe(_ => ReturnObject(audioSource));
            }

            return audioSource;
        }


        public override void Init()
        {
            
        }

        public override void PlayClip(AudioClip audioClip, bool isLoop, float volume)
        {
        }

        public override void MuteClip(AudioClip audioClip, bool isMuted)
        {
            var audioSource = _audioSourcePool.FindObjectByAudioClip(audioClip);
            if (audioSource == null)
            {
                return;
            }
            
            audioSource.mute = isMuted;
            ReturnObject(audioSource);
        }
        
        public void MuteClip(AudioSource audioSource, bool isMuted)
        {
            audioSource.mute = isMuted;
            ReturnObject(audioSource);
        }

        private void ReturnObject(AudioSource audioSource)
        {
            if(audioSource == null) return;
            audioSource.transform.parent = transform;
            _audioSourcePool.ReturnObject(audioSource);
        }

        public override void MuteAll(bool isOn)
        {
            foreach (var audioSource in _audioSourcePool.GetPoolList())
            {
                audioSource.mute = !isOn;
            }
        }
    }
}