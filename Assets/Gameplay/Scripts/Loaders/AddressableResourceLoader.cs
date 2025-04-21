using System;
using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;

namespace Gameplay.Scripts.Loaders
{
    public class AddressableResourceLoader : IResourceLoader
    {

        public AsyncOperationHandle CurrentOperation { get; private set; }

        private SignalBus _signalBus;
        private UIManager _uiManager;

        [Inject]
        public void Construct(SignalBus signalBus,UIManager uiManager)
        {
            _uiManager = uiManager;
            _signalBus = signalBus;
        }

        public async UniTask<(bool, long)> IsResourceDownloaded(object reference)
        {
            var downloadedSize = await Addressables.GetDownloadSizeAsync(reference);
            return (Mathf.Approximately(downloadedSize, 0), downloadedSize);
        }

        public async UniTask<T> LoadResourceAsync<T>(object reference) where T : Object
        {
            var assetReference = (AssetReference) reference;
            Debug.Log($"[AddressableResourceLoader] LoadResourceAsync - Name: {assetReference.AssetGUID}, Type: {typeof(T)}");
            var result = await IsResourceDownloaded(reference);
            
            if (!result.Item1) 
            {
                CurrentOperation = Addressables.DownloadDependenciesAsync(reference);
                 await CurrentOperation.ToUniTask
                (Progress.Create<float>(
                    percent => Debug.LogError(percent))
                );
            }
            if (assetReference.Asset != null)
            {
                Debug.Log($"[AddressableResourceLoader] LoadResourceAsync - Return from cache: {assetReference.Asset.name}");
                return (T)assetReference.Asset;
            }
            var handle = assetReference.LoadAssetAsync<T>();
            // if (!handle.IsDone && _uiManager.GetCurrentScreen().GetType() != typeof(LoadingScreenController))
            // {
            //     await _uiManager.Show<LoadingScreenController>();
            //     _signalBus.TryFire(new LoadingProgressSignal(handle.PercentComplete));
            // }
            await handle;
            //_uiManager.Hide<LoadingScreenController>();
            Debug.Log($"[AddressableResourceLoader] LoadResourceAsync - LoadAssetAsync Type: {typeof(T)}, Name: {handle.Result.name}");

            return handle.Result;
        }

        public void UnloadResource(object reference)
        {
            var assetReference = (AssetReference) reference;

            if (assetReference.Asset == null) return;

            assetReference.ReleaseAsset();
        }
        
        public async UniTask TryPreloadResource(AssetReference reference)
        {
            var result = await IsResourceDownloaded(reference);
            
            if (result.Item1) return;

            while ((await PreloadResource(reference)) == false)
            {
                await UniTask.Yield();
            }
        }

        public T TryGetFromCache<T>(object reference) where T : Object
        {
            var assetReference = (AssetReference) reference;

            if (assetReference.Asset != null)
            {
                return (T)assetReference.Asset;
            }

            return null;
        }

        public async UniTask<GameObject> InstantiateAsync(object reference, Transform parent)
        {
            return await Addressables.InstantiateAsync(reference, parent);
        }

        public bool ReleaseInstance(GameObject instance)
        {
            return Addressables.ReleaseInstance(instance);
        }

        private async UniTask<bool> PreloadResource(object reference)
        {

            GameObject result;

            try
            {
                result = await LoadResourceAsync<GameObject>(reference);
            }
            catch (Exception e)
            {
                Debug.LogError($"[AddressablePreloader] Error while loading resource from reference {reference}." +
                               $"Error: {e.Message}");
                return false;
            }
            
            return result != null;
        }
    }
}