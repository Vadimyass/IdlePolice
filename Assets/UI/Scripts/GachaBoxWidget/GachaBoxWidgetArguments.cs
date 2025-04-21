using Gameplay.Configs;
using UI.Scripts.ShopWindow.Gacha;
using UnityEngine;

namespace UI.Scripts.GachaBoxWidget
{
    public class GachaBoxWidgetArguments : UIArguments
    {
        public string NameKey { get; private set; }
        public Sprite Sprite { get; private set; }
        public GachaBoxType GachaBoxType { get; private set; }

        public GachaBoxWidgetArguments(GachaBoxType gachaBoxType, string nameKey, Sprite sprite)
        {
            NameKey = nameKey;
            Sprite = sprite;
            GachaBoxType = gachaBoxType;
        }
    }
}