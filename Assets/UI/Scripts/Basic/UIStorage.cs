using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public class UIStorage
    {
        private readonly UIFactory _uiFactory;
        private readonly Transform _screenParent;
        
        private readonly Dictionary<Type, UIScreenController> _cachedScreens = new Dictionary<Type, UIScreenController>();

        public UIStorage(UIFactory uiFactory, Transform screenParent)
        {
            _uiFactory = uiFactory;
            _screenParent = screenParent;
        }

        public bool HaveController<T>() where T : UIScreenController => _cachedScreens.ContainsKey(typeof(T));

        public async UniTask<T> GetScreenController<T>() where T : UIScreenController
        {
            var targetType = typeof(T);

            if (!HaveController<T>())
            {
                _cachedScreens[targetType] = await _uiFactory.Create<T>(_screenParent);
            }
            
            return (T)_cachedScreens[targetType];
        }
    }
}