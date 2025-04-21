using System;
using Gameplay.Scripts.Criminals;
using UI;

namespace Gameplay.Configs
{
    [Serializable]
    public class CriminalInitializator : IPhrase
    {
        public CriminalNames CriminalName;
        public SpriteName CriminalIcon;
        public float CriminalTime;
    }
}