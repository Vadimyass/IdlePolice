using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Configs;
using ModestTree;
using MyBox;
using RotaryHeart.Lib.SerializableDictionary;
using SolidUtilities.Collections;
using UnityEditor;
using UnityEngine;

namespace UI
{
    public class SpritesConfig : ScriptableObject
    {
        [SerializeField] public List<GameSprite> _sprites = new();

        public Sprite GetSpriteByName(SpriteName spriteName)
        {
            return _sprites.FirstOrDefault(x => x.SpriteName == spriteName).Sprite;
        }

        public Sprite GetGachaBoxSpriteByType(GachaBoxType gachaBoxType)
        {
            switch (gachaBoxType)
            {
                case GachaBoxType.Basic:
                    return GetSpriteByName(SpriteName.Basic_box);
                    break;
                case GachaBoxType.Advanced:
                    return GetSpriteByName(SpriteName.Advanced_box);
                    break;
                case GachaBoxType.Expert:
                    return GetSpriteByName(SpriteName.Expert_box);
                    break;
            }
            
            return null;
        }
    }

    [Serializable]
    public struct GameSprite
    {
        [SearchableEnum] public SpriteName SpriteName;
        public Sprite Sprite;
    }
}