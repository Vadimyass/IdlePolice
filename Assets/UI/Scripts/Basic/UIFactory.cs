using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Loaders;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace UI
{
    public class UIFactory : IDisposable
    {
        private DiContainer _container;
        private UIConfig _uiConfig;
        private IResourceLoader _resourceLoader;

        private List<AssetReference> _cachedScreens = new ();

        public UIFactory(DiContainer container, UIConfig uiConfig,IResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
            _container = container;
            _uiConfig = uiConfig;
        }

        public async UniTask<UIScreenController> Create<T>(Transform parent) where T : UIScreenController
        {
            var uiScreen = await GetUIScreen<T>(parent);
            uiScreen.gameObject.SetActive(false);

            var screenController = _container.Instantiate<T>();
            
            screenController.Init(uiScreen);
            return screenController;
        }
        
        private async UniTask<UIScreen> GetUIScreen<T>(Transform parent) where T : UIScreenController
        {
            var screenReference = _uiConfig.GetUIPrefab<T>();
            if (_cachedScreens.Contains(screenReference))
            {
                return (UIScreen)_cachedScreens.First(x => x == screenReference).Asset;
            }   
            var screen = await _resourceLoader.InstantiateAsync(screenReference, parent);
            var uiScreen = screen.GetComponent<UIScreen>();
            _cachedScreens.Add(screenReference);
            _container.InjectGameObject(uiScreen.gameObject);
            return uiScreen;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}