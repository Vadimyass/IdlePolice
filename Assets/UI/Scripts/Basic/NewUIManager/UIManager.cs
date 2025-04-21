using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Utils;
using UI.Stack;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIManager : MonoBehaviour , IDisposable
    {
        private UIStorage _uiStorage;
        [SerializeField] private Transform _root;
        [SerializeField] private Canvas _rootCanvas;
        private bool _isScreenAwait;
        public UiFilter UiFilter { get; } = new UiFilter();

        private Dictionary<UIType, Stack<UIScreenController>> _uiStacks = new()
        {
            { UIType.Screen ,new Stack<UIScreenController>()},
            { UIType.Window ,new Stack<UIScreenController>()},
            { UIType.Widget ,new Stack<UIScreenController>()},
        };
        
        [Inject]
        public void Construct(UIFactory uiFactory)
        {
            _uiStorage = new UIStorage(uiFactory, _root);
        }


        public void SetCanvasSortOrder(int sortOrder)
        {
            _rootCanvas.sortingOrder = sortOrder;
        }
        
        public async UniTask Show<T>(UIArguments args = null) where T : UIScreenController
        {
            if (!UiFilter.CanCreateCommand(typeof(T))) return;
            await UniTask.WaitUntil(() => !_isScreenAwait);
            _isScreenAwait = true;
            var screenController = await _uiStorage.GetScreenController<T>();
            if (_uiStacks.Any(x => x.Value.Contains(screenController)) == false)
            {
                _uiStacks[screenController.UIType].TryPeek(out var screen);
                screen?.OnHide();
                _uiStacks[screenController.UIType].Push(screenController);
                switch (screenController.UIType)
                {
                    case UIType.Screen:
                        _uiStacks[UIType.Window].Clear();
                        break;
                    case UIType.Window:
                        _uiStacks[UIType.Widget].Clear();
                        break;
                    case UIType.Widget:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            await screenController.Display(args);
            screenController.UIScreen.transform.SetAsLastSibling();
            _isScreenAwait = false;
        }


        public async UniTask<bool> HideLast()
        {
            BigDDebugger.LogError(_uiStacks[UIType.Widget].Count, _uiStacks[UIType.Window].Count, _uiStacks[UIType.Screen].Count);
            
            if (_uiStacks[UIType.Widget].Count > 0)
            {
                var lastWidget = _uiStacks[UIType.Widget].Pop();
                await lastWidget.OnHide();
                _uiStacks[UIType.Widget].TryPeek(out var nextWidget);
                if (nextWidget != null)
                {
                    await nextWidget.UpdateScreen(); 
                }
                return true;
            }

            if (_uiStacks[UIType.Window].Count > 0)
            {
                var lastWindow = _uiStacks[UIType.Window].Pop();
                BigDDebugger.LogError(lastWindow.UIScreen.name);
                await lastWindow.OnHide();
                _uiStacks[UIType.Window].TryPeek(out var nextWindow);
                if (nextWindow != null)
                {
                    await nextWindow.UpdateScreen(); 
                }
                else
                {
                    _uiStacks[UIType.Screen].TryPeek(out var nextScreen);
                    if (nextScreen != null)
                    {
                        await nextScreen.UpdateScreen(); 
                    }
                }
                return true;
            }

            return false;
        }


        public async UniTask HideCurrentWindow()
        {
            if (_uiStacks[UIType.Window].Count > 0)
            {
                var lastWindow = _uiStacks[UIType.Window].Pop();
                await lastWindow.OnHide();
            }
        }

        public async UniTask HideCurrentWidget()
        {
            Debug.LogError(_uiStacks[UIType.Widget].Count);
            if (_uiStacks[UIType.Widget].Count > 0)
            {
                var lastWidget = _uiStacks[UIType.Widget].Pop();
                await lastWidget.OnHide();
            }
        }

        public async UniTask HideCurrentScreen()
        {
            if (_uiStacks[UIType.Screen].Count > 0)
            {
                var lastWidget = _uiStacks[UIType.Screen].Pop();
                await lastWidget.OnHide();
            }
        }
        
        public void Dispose()
        {
            
        }

        public bool IsActiveAnyWindowOrWidget()
        {
            return _uiStacks[UIType.Window].Count + _uiStacks[UIType.Widget].Count > 0;
        }
    }
}