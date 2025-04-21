using Gameplay.OrderManaging;
using UnityEngine;
using Zenject;

namespace Installers.Scripts
{
    public class OrderManagementInstaller : ScriptableObjectInstaller<OrderManagementInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<OrderManager>().FromNew().AsSingle().NonLazy();
        }
    }
}