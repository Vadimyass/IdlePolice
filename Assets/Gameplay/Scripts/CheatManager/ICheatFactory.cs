using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Scripts.CheatManager
{
    public interface ICheatFactory
    {
        UniTask<CheatItemBase> GetItem<T>(Transform transform) where T: CheatItemBase;
    }
}