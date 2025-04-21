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
    public class SpritesConfigDictionaries : ScriptableObject
    {
        [SerializeField] public SerializableDictionary<GachaBoxType, BoxSprites> _boxSprites = new();

        public BoxSprites GetBoxSprites(GachaBoxType gachaBoxType)
        {
            return _boxSprites.FirstOrDefault(x => x.Key == gachaBoxType).Value;
        }
    }

    [Serializable]
    public struct BoxSprites
    {
        public Sprite ClosedSprite;
        public Sprite OpenSprite;
        public Sprite TopSprite;
    }
}