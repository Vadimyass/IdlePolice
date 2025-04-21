using Gameplay.Scripts.Loaders;
using Zenject;

namespace Installers.Scripts
{
    public class AddressablesInstaller : ScriptableObjectInstaller<AudioInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IResourceLoader>().To<AddressableResourceLoader>().FromNew().AsSingle();
        }
    }
}