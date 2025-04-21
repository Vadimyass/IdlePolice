using System;
using System.Collections.Generic;
using System.Threading;
using Audio;
using Configs;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Audio;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Utils;
using MyBox;
using SolidUtilities.Collections;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private MusicPlayer _musicPlayer;
    [SerializeField] private SoundPlayer _soundPlayer;
    [SerializeField] private WorldSoundPlayer _worldSoundPlayer;
    private AudioConfig _audioConfig;
    private SignalBus _signalBus;
    private PlayerPrefsSaveManager _playerPrefsData;

    private SettingsModel Settings => _playerPrefsData.PrefsData.SettingsModel;
    private bool IsMusicEnabled => Settings.IsSettingEnabled(SettingType.Music);
    private bool IsSoundEnabled => Settings.IsSettingEnabled(SettingType.Sound);

    private CancellationTokenSource _cancellationTokenSource;

    [Inject]
    private void Construct(AudioConfig audioConfig, PlayerPrefsSaveManager playerPrefsData, SignalBus signalBus)
    {
        _playerPrefsData = playerPrefsData;
        _signalBus = signalBus;
        _audioConfig = audioConfig;

        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void Init()
    {
        foreach (var model in ReflectionUtils.GetFieldsOfType<AudioPlayer>(this))
        {
            model.Init();
        }
    }

    public void SetMusicVolume(float volume)
    {
        _musicPlayer.SetVolume(volume);
    }
    
    public async UniTask PlayMusic(TrackName trackName,float volume = 1, bool isLoop = true)
    {
        await PlayTrack(trackName, _musicPlayer, isLoop, volume);
    }

    public async UniTask PlaySound(TrackName trackName, bool isLoop = false, float volume = 1)
    {
        await PlayTrack(trackName, _soundPlayer, isLoop, volume);
    }
    
    public AudioSource PlayWorldSound(TrackName trackName, Transform parent, bool isLoop = false)
    {
        if (_worldSoundPlayer == null)
        {
            return null;
        }
        var audioClip = _audioConfig.GetAudioConfig(trackName);
        if (audioClip == null)
        {
            return null;
        }
        _cancellationTokenSource.Cancel();
        var source = _worldSoundPlayer.PlayClip(audioClip, isLoop, parent);
        return source;
    }


    public void PlayDialogSound(TrackName trackName)
    {
        var audioClip = _audioConfig.GetAudioConfig(trackName);
        if(trackName == default || audioClip == null) return;
        
        _soundPlayer.PlayDialogClip(audioClip);
    }

    private async UniTask PlayTrack(TrackName trackName, AudioPlayer audioPlayer, bool isLoop, float volume)
    {
        if (audioPlayer == null)
        {
            return;
        }
        var audioClip = _audioConfig.GetAudioConfig(trackName);
        if (audioClip == null)
        {
            return;
        }
        _cancellationTokenSource.Cancel();
        audioPlayer.PlayClip(audioClip, isLoop, volume);
        await UniTask.Delay(TimeSpan.FromSeconds(audioClip.length)).AttachExternalCancellation(_cancellationTokenSource.Token).SuppressCancellationThrow();
    }

    public float GetDurationOfTrackByName(TrackName trackName)
    {
        var audioClip = _audioConfig.GetAudioConfig(trackName);
        return audioClip.length;
    }

    public void MuteMusicPlayer(bool isOn)
    {
        _musicPlayer.MuteClip(null, isOn);
    }

    public void MuteSoundPlayer(bool isOn)
    {
        _soundPlayer.MuteAll(isOn);
        _worldSoundPlayer.MuteAll(isOn);
    }

    public void TurnOffSound(TrackName trackName, bool isMuted)
    {
        var audioClip = _audioConfig.GetAudioConfig(trackName);
        if (audioClip == null)
        {
            return;
        }
        _soundPlayer.MuteClip(audioClip, isMuted);
    }
    
    public void TurnOffWorldSound(AudioSource audioSource, bool isMuted)
    {
        _worldSoundPlayer.MuteClip(audioSource, isMuted);
    }
}

[Serializable]
public struct EnemySounds
{
    public int ID;
    [SearchableEnum] public List<TrackName> TrackNames;
}