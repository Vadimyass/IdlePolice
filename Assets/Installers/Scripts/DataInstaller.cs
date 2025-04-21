using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using UnityEngine;
using Zenject;

public class DataInstaller : ScriptableObjectInstaller<DataInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerPrefsSaveManager>().FromNew().AsSingle().NonLazy();
    }
}