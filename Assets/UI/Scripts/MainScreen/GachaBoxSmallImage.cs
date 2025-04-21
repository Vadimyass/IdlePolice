using UnityEngine;
using UnityEngine.UI;
using Zenject;
using IPoolable = Pool.IPoolable;

namespace UI.Scripts.MainScreen
{
    public class GachaBoxSmallImage : Image, IPoolable
    {
        public void Return()
        {
            
        }

        public void Release()
        {
            
        }
    }
}