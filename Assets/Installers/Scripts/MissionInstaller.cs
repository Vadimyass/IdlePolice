using Gameplay.Configs;
using Gameplay.Scripts.Missions;
using UnityEngine;
using Zenject;

namespace Installers.Scripts
{
    public class MissionInstaller : ScriptableObjectInstaller<AudioInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<MissionController>().FromNew().AsSingle().NonLazy();
        }
    }
}