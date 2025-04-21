using Gameplay.Configs;
using UnityEngine;
using Zenject;

namespace Installers.Scripts
{
    public class LocalizatorInstaller : ScriptableObjectInstaller<AudioInstaller>
    {
        [SerializeField] private LocalizationManager _localizationManager;
        public override void InstallBindings()
        {
            Container.Bind<LocalizationManager>().FromComponentInNewPrefab(_localizationManager).AsSingle().NonLazy();
        }
    }
}