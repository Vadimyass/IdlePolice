using Gameplay.Scripts.Locker;
using Zenject;

namespace Installers.Scripts
{
    public class LockControllerInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LockController>().FromNew().AsSingle();
        }
    }
}