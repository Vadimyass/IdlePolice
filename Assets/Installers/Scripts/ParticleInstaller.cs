using Particles;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ParticleInstaller", menuName = "Installers/ParticleInstaller")]
public class ParticleInstaller : ScriptableObjectInstaller<ParticleInstaller>
{
    [SerializeField] private ParticleManager _particleManager;
    [SerializeField] private ParticleConfig _particleConfig;
    
    public override void InstallBindings()
    {
        Container
            .Bind<ParticleFactory>()
            .FromNew()
            .AsSingle()
            .WithArguments(_particleConfig);
        Container
            .BindInterfacesAndSelfTo<ParticleManager>()
            .FromComponentInNewPrefab(_particleManager)
            .AsSingle();
    }
}