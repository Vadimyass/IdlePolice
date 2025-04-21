using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Loaders;
using Gameplay.Scripts.Utils;
using LunarConsolePlugin;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.CheatManager
{
    public class CheatManager : MonoBehaviour , ICheatManager
    {
        private readonly HashSet<CheatItemBase> _cheatItems = new HashSet<CheatItemBase>();

        private CheatWindow _cheatWindow;
        private List<CheatPopupBase> _popups;
        
        private ICheatFactory _factory;
        
        private IResourceLoader _resourceLoader;
        private CheatManagerConfig _cheatManagerConfig;
        private CheatConfig _cheatConfig;
        private DiContainer _diContainer;
        
        //private bool EditorInput() => Input.GetKeyUp(KeyCode.E);
        private bool EditorInput() => Input.GetKeyUp(KeyCode.F1);
        private bool DeviceInput() => Input.touchCount == 4;

        private bool ConsoleInput() => Input.GetKeyUp(KeyCode.R);
        //private bool ConsoleInput() => Input.GetKeyUp(KeyCode.F2);

        [Inject]
        public void Construct(
            IResourceLoader resourceLoader,
            CheatManagerConfig cheatManagerConfig,
            CheatConfig cheatConfig,
            DiContainer diContainer)
        {
            _resourceLoader = resourceLoader;
            _cheatManagerConfig = cheatManagerConfig;
            _cheatConfig = cheatConfig;
            _diContainer = diContainer;
        }

        public async void Awake()
        {
            _factory = new CheatFactoryResourceLoad();
            var cheatFactoryResourceLoad = (CheatFactoryResourceLoad) _factory;
            cheatFactoryResourceLoad.Setup(_resourceLoader, _cheatManagerConfig);

            await InitializeCheatView();
            
            var types = _cheatConfig.GetCheatTypes();
            foreach (var cheat in types.Select(type => (ICheat)_diContainer.Instantiate(type)))
            {
                cheat.AddCheat();
            }
        }

        public async void AddCheat<T>(Action<T> action) where T : CheatItemBase
        {
            var newInstance = await _factory.GetItem<T>(_cheatWindow.Content) as T;
            newInstance.Configurate(this);
            action(newInstance);

            _cheatItems.Add(newInstance);
        }

        public void AddPopup<T>(Action<T> action, string buttonName) where T : CheatPopupBase
        {
            foreach (var newInstance in from popup in _popups where popup.GetType() == typeof(T) select popup as T)
            {
                newInstance.Configurate(this);
                action(newInstance);
                newInstance.gameObject.SetActive(false);
            
                AddCheat<CheatButtonPopup>(button => button
                    .SetPopup(newInstance).SetButtonName(buttonName)
                );

                _cheatItems.Add(newInstance);
                break;
            }
        }

        public void Show()
        {
            _cheatWindow.gameObject.SetActive(true);
        }
        public void Close()
        {
            _cheatWindow.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            var isActive = false;
            
            isActive = EditorInput() || DeviceInput();

            if (ConsoleInput())
            {
                //LunarConsole.Show();
            }
            if (isActive)
            {
                Show();
            }
        }

        private async UniTask InitializeCheatView()
        {
            var windowPrefab = await _resourceLoader.LoadResourceAsync<GameObject>(_cheatManagerConfig.CheatWindowReference);
            _cheatWindow = _diContainer.InstantiatePrefabForComponent<CheatWindow>(windowPrefab, transform);
            _cheatWindow.gameObject.SetActive(false);

            _popups = new List<CheatPopupBase>();

            for (int i = 0; i < _cheatManagerConfig.PopupsReferences.Count; i++)
            {
                var popupPrefab = await _resourceLoader.LoadResourceAsync<GameObject>(_cheatManagerConfig.PopupsReferences[i]);
                var popup = _diContainer.InstantiatePrefabForComponent<CheatPopupBase>(popupPrefab, transform);
                
                _popups.Add(popup);
            }
        }
    }
}