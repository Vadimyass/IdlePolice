using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Loaders;
using UnityEngine;

namespace Gameplay.Scripts.CheatManager
{
    public class CheatFactoryResourceLoad : ICheatFactory
    {
        private IResourceLoader _resourceLoader;
        private CheatManagerConfig _cheatManagerConfig;
        
        public void Setup(IResourceLoader resourceLoader, CheatManagerConfig cheatManagerConfig)
        {
            _resourceLoader = resourceLoader;
            _cheatManagerConfig = cheatManagerConfig;
        }
        
        public async UniTask<CheatItemBase> GetItem<T>(Transform transform) where T: CheatItemBase
        {
            var assetReference = _cheatManagerConfig.GetReference<T>();

            var prefab = await _resourceLoader.InstantiateAsync(assetReference, transform);
            return prefab.GetComponent<CheatItemBase>();
        }
    }
}