using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.Basic
{
    public class SpriteWithRarityStates : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private List<SpriteState> _spriteStates;

        public void ChangeState(OfficerRarity officerRarity)
        {
            _image.sprite = _spriteStates.FirstOrDefault(x => x.Rarity == officerRarity).Sprite;
        }
    }

    [Serializable]
    public struct SpriteState
    {
        public OfficerRarity Rarity;
        public Sprite Sprite;
    }
}