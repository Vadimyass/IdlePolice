using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.LevelManagement;
using UnityEngine;
using Zenject;

namespace Installers.Scripts
{
    public class LevelInstaller : ScriptableObjectInstaller<LevelInstaller>
    {
        [SerializeField] private LevelConfig _levelConfig;
        public override void InstallBindings()
        {
            Container.Bind<LevelConfig>().FromInstance(_levelConfig).AsSingle().NonLazy();
            Container.Bind<LevelController>().FromNew().AsSingle().NonLazy();
            Container.Bind<BaseUpgradesController>().FromNew().AsSingle().NonLazy();
        }
    }
}