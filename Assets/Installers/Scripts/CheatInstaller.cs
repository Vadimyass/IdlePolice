using Gameplay.Scripts.CheatManager;
using UnityEngine;
using Zenject;

namespace Installers.Scripts
{
    public class CheatInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private CheatManager _cheatManagerPrefab;

        [SerializeField] private CheatManagerConfig _cheatManagerConfig;
        [SerializeField] private CheatConfig _cheatConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<ICheatManager>().To<CheatManager>().FromComponentInNewPrefab(_cheatManagerPrefab)
                .AsSingle().WithArguments(_cheatManagerConfig, _cheatConfig).NonLazy();
        }
    }
}