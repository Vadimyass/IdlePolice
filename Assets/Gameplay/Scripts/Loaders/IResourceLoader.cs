using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Gameplay.Scripts.Loaders
{
    public interface IResourceLoader
    {
        AsyncOperationHandle CurrentOperation { get; }
        UniTask<(bool, long)> IsResourceDownloaded(object reference);
        UniTask<T> LoadResourceAsync<T>(object reference) where T : Object;
        void UnloadResource(object reference);
        T TryGetFromCache<T>(object reference) where T : Object;
        UniTask<GameObject> InstantiateAsync(object reference, Transform parent);
        bool ReleaseInstance(GameObject instance);
        UniTask TryPreloadResource(AssetReference reference);
    }
}