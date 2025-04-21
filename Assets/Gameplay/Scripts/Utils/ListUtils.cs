using System.Collections.Generic;
using System.Linq;

namespace Gameplay.Scripts.Utils
{
    public static class ListUtils
    {
        public static List<List<T>> DivideBy<T>(this List<T> values, int chunkSize)
        {
            return values.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}