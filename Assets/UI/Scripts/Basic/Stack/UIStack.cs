using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Utils;
using UnityEngine;

namespace UI.Stack
{
    public class UIStack : IDisposable
    {
        private readonly List<UIStackItem> _stackItems = new List<UIStackItem>();
        private readonly List<UICommand> _commands = new List<UICommand>();

        public bool HaveCommands => _commands.Count > 0;
        public int Size => _stackItems.Count;
        internal UIStackItem CurrentUIStackItem => _stackItems.Count > 0 ? _stackItems.Last() : null;

        private UICommand ProcessingCommand { get; set; }

        public bool IsAnyCommandInStack()
        {
            foreach (var stackItem in _stackItems)
            {
                if (stackItem.Stack.IsAnyCommandInStack())
                {
                    return true;
                }
            }
            
            return HaveCommands;
        }

        public bool IsCurrentStackItemInStack<T>() where T: UIScreenController
        {
            return _stackItems.Any(stackItem => stackItem.ScreenController.GetType() == typeof(T));
        }

        public UIScreenController GetScreen<T>() where T : UIScreenController
        {
            return _stackItems.FirstOrDefault(stackItem => stackItem.ScreenController.GetType() == typeof(T))?.ScreenController;
        }
        
        public bool IsCurrentStackItemInStack(UIScreenController screenController)
        {
            return _stackItems.Any(stackItem => stackItem.ScreenController.GetType() == screenController.GetType());
        }

        private void Reset()
        {
            foreach (var window in _stackItems)
            {
                window.ScreenController.OnHide();
                window.Stack.Reset();
            }
            
            _stackItems.Clear();
            _commands.Clear();
        }

        internal void AddCommand(UICommand command)
        {
            _commands.Add(command);
        }

        internal async UniTask Process()
        {
            foreach (var window in _stackItems)
            {
                await window.Stack.Process();
            }
            
            if (_commands.Count > 0)
            {
                var command = _commands.First();
                ProcessingCommand = command;
                _commands.Remove(command);
                
                switch (command.CommandType)
                {
                    case UICommand.Type.Show:
                        await ShowCommand(command);
                        break;
                    
                    case UICommand.Type.Hide:
                        await HideCommand(command);
                        break;
                }

                ProcessingCommand = null;
            }
        }

        private async UniTask ShowCommand(UICommand command)
        {
            
            if (CurrentUIStackItem != null 
                && CurrentUIStackItem.ScreenController.UIScreen.IsShown
                && CurrentUIStackItem.ScreenController.UIScreen.UIType == UIType.Screen)
            {
                await Hide(CurrentUIStackItem);
            }
            
            var stackItem = new UIStackItem( command.UIScreenController, command.UIScreenArgs );
            
            _stackItems.Add(stackItem);
            
            await Show(stackItem);
        }

        private async UniTask HideCommand(UICommand command)
        {
            if (CurrentUIStackItem?.ScreenController == command.UIScreenController)
            {
                var isScreen = CurrentUIStackItem.ScreenController.UIScreen.UIType == UIType.Screen;
                await Hide(CurrentUIStackItem);
                CurrentUIStackItem.Stack.Reset();
                RemoveLast(_stackItems);
                if (isScreen)
                {
                    await Show(CurrentUIStackItem);
                }
            } else if (IsCurrentStackItemInStack(command.UIScreenController))
            {
                var targetStackItem = _stackItems.First(stackItem =>
                    stackItem.ScreenController.GetType() == command.UIScreenController.GetType());
                await Hide(targetStackItem);
                targetStackItem.Stack.Reset();
                _stackItems.Remove(targetStackItem);
            }
        }

        private async UniTask Hide(UIStackItem stackItem)
        {
            var screenController = stackItem.ScreenController;
            if (screenController != null)
            {
                if (stackItem.Stack.CurrentUIStackItem != null)
                {
                    await Hide(stackItem.Stack.CurrentUIStackItem);
                }
            
                await screenController.OnHide();
                ResetProcessingCommand(screenController);
            }
        }

        private async UniTask Show(UIStackItem stackItem)
        {
            if (stackItem != null)
            {
                var uiScreen = stackItem.ScreenController.UIScreen;
                uiScreen.gameObject.transform.SetSiblingIndex(999);
                
                ResetProcessingCommand(stackItem.ScreenController, UICommand.Type.Show);

                await stackItem.ScreenController.Display(stackItem.UIScreenArgs);
                
                if (stackItem.Stack.CurrentUIStackItem != null)
                {
                    await Show(stackItem.Stack.CurrentUIStackItem);
                }
            }
        }

        private void ResetProcessingCommand(UIScreenController uiScreenController, params UICommand.Type[] commands)
        {
            if (ProcessingCommand == null || ProcessingCommand.UIScreenController != uiScreenController) return;
            
            if (commands.Contains(ProcessingCommand.CommandType))
            {
                ProcessingCommand = null;
            }
        }
        
        private void RemoveLast<T>(List<T> list)
        {
            if (list != null && list.Count > 0)
            {
                list.RemoveAt(list.Count - 1);
            }
        }

        public void Dispose()
        {
            foreach (var window in _stackItems)
            {
                window.Stack.Dispose();
            }
            
            _stackItems.Clear();
            _commands.Clear();
        }
    }
}