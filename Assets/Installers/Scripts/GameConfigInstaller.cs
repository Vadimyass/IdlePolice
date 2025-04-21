using BigD.Config;
using UnityEngine;
using Zenject;

namespace Installers.Scripts
{
    public class GameConfigInstaller : ScriptableObjectInstaller<GameConfigInstaller>
    {
        [SerializeField] private GameConfig _gameConfig;
        
        public override void InstallBindings()
        {
            Container
                .Bind<GameConfig>()
                .FromInstance(_gameConfig)
                .AsSingle();
        }
    }
}