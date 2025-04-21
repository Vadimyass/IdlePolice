using Gameplay.Configs;
using UI.Scripts.ShopWindow.Gacha;
using UnityEngine;

namespace UI.Scripts.GachaBoxOpenWindow
{
    public class GachaBoxOpenWindowArguments : UIArguments
    {
        public int Count { get; private set; }
        public string NameKey { get; private set; }
        public Sprite Sprite { get; private set; }
        public GachaBoxType GachaBoxType { get; private set; }

        public GachaBoxOpenWindowArguments(GachaBoxType gachaBoxType, string nameKey, Sprite sprite, int count)
        {
            Count = count;
            NameKey = nameKey;
            Sprite = sprite;
            GachaBoxType = gachaBoxType;
        }
    }
}