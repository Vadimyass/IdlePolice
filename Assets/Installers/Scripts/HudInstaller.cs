using UI.Huds.Scripts;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class HudInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private HudManager _hudManager;

        public override void InstallBindings()
        {
            Container.Bind<HudManager>().FromComponentInNewPrefab(_hudManager).AsSingle().NonLazy();
        }
    }
}