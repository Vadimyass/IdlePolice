using Configs;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "AudioInstaller", menuName = "Installers/AudioInstaller")]
public class AudioInstaller : ScriptableObjectInstaller<AudioInstaller>
{
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private AudioConfig _audioConfig;
    public override void InstallBindings()
    {
        Container
            .Bind<AudioConfig>()
            .FromInstance(_audioConfig)
            .AsSingle();
        Container
            .BindInterfacesAndSelfTo<AudioManager>()
            .FromComponentInNewPrefab(_audioManager)
            .AsSingle()
            .NonLazy();
    }
}