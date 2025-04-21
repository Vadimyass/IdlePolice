using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class MusicPlayer : AudioPlayer
    {
        [SerializeField] private AudioSource _audioSource;
        private PlayerPrefsSaveManager _playerPrefsData;
        private float _volume;
        private bool IsMuted => _playerPrefsData == null || !_playerPrefsData.PrefsData.SettingsModel.IsSettingEnabled(SettingType.Music);

        [Inject]
        private void Construct(PlayerPrefsSaveManager playerPrefsData)
        {
            _playerPrefsData = playerPrefsData;
        }

        public override void Init()
        {
            _volume = _audioSource.volume;
        }

        public void SetVolume(float volumeValue)
        {
            _audioSource.outputAudioMixerGroup.audioMixer.SetFloat("MasterVolume", Mathf.Log10(volumeValue/100) * 20);
        }
        
        public override void PlayClip(AudioClip audioClip, bool isLoop, float volume)
        {
            if (IsMuted) return;
            
            var seq = DOTween.Sequence();
            seq.Append(_audioSource.DOFade(0, 0.5f));
            seq.AppendCallback(() =>
            {
                _audioSource.clip = audioClip;
                _audioSource.loop = isLoop;
                _audioSource.Play();
            });
            seq.Append(_audioSource.DOFade(volume, 0.5f));
        }


        public override void MuteClip(AudioClip audioClip, bool isOn)
        {
            _audioSource.mute = !isOn;
        }

        public override void MuteAll(bool isMuted)
        {
        }
    }
}