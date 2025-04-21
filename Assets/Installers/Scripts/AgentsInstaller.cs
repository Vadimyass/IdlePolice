using Agents;
using Gameplay.Agents;
using UnityEngine;
using Zenject;

namespace Gameplay.Installers
{
    public class AgentsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private AgentsConfig _agentsConfig;
        public override void InstallBindings()
        {
            Container.Bind<AgentsManager>().FromNew().AsSingle().WithArguments(_agentsConfig).NonLazy();
        }
    }
}