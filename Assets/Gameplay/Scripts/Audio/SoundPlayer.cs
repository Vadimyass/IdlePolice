using System;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Pool;
using UniRx;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class SoundPlayer : AudioPlayer
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

        public override void Init()
        {
            MuteClip(null, IsMuted);
        }

        public override void PlayClip(AudioClip audioClip, bool isLoop, float volume)
        {
            if (IsMuted) return;

            var audioSource = _audioSourcePool.GetObject();
            audioSource.volume = volume;
            audioSource.mute = false;
            audioSource.clip = audioClip;
            audioSource.loop = isLoop;
            audioSource.Play();

            if (audioSource.loop == false)
            {
                int timeWait = (int)Math.Ceiling(audioClip.length);
                Observable.Timer(TimeSpan.FromSeconds(timeWait)).Subscribe(_ => _audioSourcePool.ReturnObject(audioSource));
            }
        }
        
        
        public void PlayDialogClip(AudioClip audioClip)
        {
            var audioSource = _audioSourcePool.GetObject();
            audioSource.mute = false;
            audioSource.clip = audioClip;
            audioSource.loop = false;
            audioSource.Play();
        }

        public override void MuteClip(AudioClip audioClip, bool isMuted)
        {
            var audioSource = _audioSourcePool.FindObjectByAudioClip(audioClip);
            if (audioSource == null)
            {
                return;
            }

            audioSource.mute = isMuted;
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