/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Utils;
using MyBox;
using UI.Stack;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class NewUIManager1 : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform _root;
        [SerializeField] private GameObject _blockPanel;

        private bool _isStackProcessing = false;
        private bool _isScreenAwait = false;
        
        private UIStorage _uiStorage;
        private UIStack _stack;
        
        public UiFilter UiFilter { get; } = new UiFilter();
        public UiFilter UiPanelFilter { get; } = new UiFilter();
        
        [Inject]
        public void Construct(UIFactory uiFactory)
        {
            _uiStorage = new UIStorage(uiFactory, _root);
            _stack = new UIStack();
        }

        public async UniTask Show<T>(UIArguments args = null) where T : UIScreenController
        {
            if (!UiFilter.CanCreateCommand(typeof(T))) return;

            await UniTask.WaitUntil(() => !_isScreenAwait);
            _isScreenAwait = true;
            var screenController = await _uiStorage.GetScreenController<T>();
            _isScreenAwait = false;
            
            var command = new UICommand(UICommand.Type.Show, screenController, args);
            
            switch (screenController.UIType)
            {
                case UIType.Screen:
                    _stack.AddCommand(command);
                    break;
                case UIType.Window:
                    if (!IsScreenOpened<T>())
                    {
                        var stack = _stack.CurrentUIStackItem?.Stack ?? _stack;
                        stack.AddCommand(command);
                    }
                    break;
                case UIType.Widget:
                    screenController.Display(args);
                    break;
            }

            ProcessStack();
        }

        public void HideLast()
        {
            UIStack stack = null;
            UIScreenController screenController = null;

            UpdateLastScreenSettings(ref stack, ref screenController);
            
            if (stack == null) return;
            stack.AddCommand(new UICommand(UICommand.Type.Hide, screenController));

            ProcessStack();
        }

        public bool TryHideLastPanel()
        {
            UIStack stack = null;
            UIScreenController screenController = null;

            UpdateLastScreenSettings(ref stack, ref screenController);

            if (stack == null) return false;
            
            if (!IsLastPanelHideAvailable(screenController)) return false;
            stack.AddCommand(new UICommand(UICommand.Type.Hide, screenController));

            ProcessStack();
            return true;
        }

        public async Task Hide<T>() where T : UIScreenController
        {
            if (!_uiStorage.HaveController<T>()) return;
            var screenController = await _uiStorage.GetScreenController<T>();
            
            switch (screenController.UIType)
            {
                case UIType.Screen:
                    _stack.AddCommand(new UICommand(UICommand.Type.Hide, screenController));
                    break;
                case UIType.Window:
                    var stack = _stack.CurrentUIStackItem?.Stack ?? _stack;
                    stack.AddCommand(new UICommand(UICommand.Type.Hide, screenController));
                    break;
                case UIType.Widget:
                    screenController.OnHide();
                    break;
            }
            
            ProcessStack();
        }

        public bool IsScreenOpened<T>() where T : UIScreenController
        {
            if (_stack.Size == 0) return false;

            if (_stack.IsCurrentStackItemInStack<T>()) return true;

            return _stack.CurrentUIStackItem.Stack.Size != 0 && _stack.CurrentUIStackItem.Stack.IsCurrentStackItemInStack<T>();
        }

        public UIScreenController GetScreen<T>() where T : UIScreenController
        {
            if (!IsScreenOpened<T>()) return null;

            var screen = _stack.GetScreen<T>();
            return screen ?? _stack.CurrentUIStackItem.Stack.GetScreen<T>();
        }

        public bool IsAnyCommandInStack()
        {
            return _isStackProcessing || _stack.IsAnyCommandInStack();
        }

        private bool IsLastPanelHideAvailable(UIScreenController screenController)
        {
            return screenController.UIType == UIType.Window &&
                   UiPanelFilter.CanCreateCommand(screenController.GetType());
        }

        private void UpdateLastScreenSettings(ref UIStack stack, ref UIScreenController screenController)
        {
            if (_stack.Size == 1 && _stack.CurrentUIStackItem.Stack.Size == 0)
            {
                return;
            }
            
            var currentTopStackItem = _stack.CurrentUIStackItem;
            
            if (currentTopStackItem == null) return;
            
            var nestedStackItem = currentTopStackItem.Stack.CurrentUIStackItem;
            
            stack = nestedStackItem != null ? currentTopStackItem.Stack : _stack;
            screenController = nestedStackItem != null ? nestedStackItem.ScreenController : currentTopStackItem.ScreenController;
        }

        private async UniTask ProcessStack()
        {
            if (_isStackProcessing) return;

            _isStackProcessing = true;
            Block(true);

            while (_stack.IsAnyCommandInStack())
            {
                await _stack.Process();
            }
            Block(false);
            _isStackProcessing = false;
        }
        
        private void Block(bool value)
        {
            _blockPanel.SetActive(value);
        }

        public void Dispose()
        {
            _stack?.Dispose();
        }
    }
}
*/
