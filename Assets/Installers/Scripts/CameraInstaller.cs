using Gameplay.Scripts;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class CameraInstaller : MonoInstaller<CameraInstaller>
{
    [SerializeField] private CameraConfig _cameraConfig;
    //[SerializeField] private AudioMixer _audioMixerGroup;
    public override void InstallBindings()
    {
        Container.Bind<CameraController>().FromNew().AsSingle().WithArguments(_cameraConfig);
        Container.Bind<CameraConfig>().FromInstance(_cameraConfig).AsSingle().NonLazy();
        Container.Bind<CameraPathFactory>().FromNew().AsSingle().WithArguments(_cameraConfig);
    }
}