using System;
using System.Collections.Generic;

namespace Gameplay.Configs
{
    [Serializable]
    public class DefaultConfigInitializer : IPhrase
    {
        public string Key;
        public List<string> Pair;
    }
}