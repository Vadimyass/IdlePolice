using System;
using UnityEngine;

namespace Gameplay.Scripts.CheatManager
{
    public class CheatItemBase : MonoBehaviour, IDisposable
    {
        protected ICheatManager _cheatManager;
        
        public void Configurate(ICheatManager cheatManager)
        {
            _cheatManager = cheatManager;
        }
        
        public virtual void Dispose()
        {
            Destroy(gameObject);
        }
    }
}