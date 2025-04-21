using System;
using System.Collections.Generic;
using Gameplay.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace Pool
{
    public abstract class BasePool<T> : IDisposable
    {
        [SerializeField] protected GameObject prefab;
        [SerializeField] protected GameObject parent;
        
        public abstract T GetObject();
        public abstract void ReturnObject(T poolObject);
        public abstract void ReleaseAll();
        public void Dispose()
        {
            ReleaseAll();
        }
    }
}