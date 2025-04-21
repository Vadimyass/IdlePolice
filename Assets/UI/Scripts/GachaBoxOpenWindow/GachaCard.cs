using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Configs;
using MyBox;
using SolidUtilities.Collections;
using TMPro;
using UI.Scripts.Basic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.GachaBoxOpenWindow
{
    public class GachaCard : MonoBehaviour
    {
        [SerializeField] private Image _mainImage;
        [SerializeField] private List<SpriteWithRarityStates> _spriteWithRarityStatesList;
        [SerializeField] private SerializableDictionary<OfficerType, Transform> _spritesTypes;
        [SerializeField] private TextMeshProUGUI _countText;
        private OfficerRarity _officerRarity;
        private OfficerType _officerType;
        public int Count { get; private set; }

        public virtual void SetInfo(Sprite mainSprite, OfficerType officerType, OfficerRarity officerRarity, int count, string nameKey)
        {
            _officerType = officerType;
            _officerRarity = officerRarity;
            _mainImage.sprite = mainSprite;
            _countText.text = "X" + count.ToString();

            foreach (var spritesType in _spritesTypes)
            {
                spritesType.Value.gameObject.SetActive(false);
            }
            
            _spritesTypes[_officerType].gameObject.SetActive(true);

            foreach (var spriteWithRarity in _spriteWithRarityStatesList)
            {
                spriteWithRarity.ChangeState(_officerRarity);
            }
            
            Count = count;
        }

        public async UniTask RevealCard(Vector3 position, Vector3 startPosition)
        {
            transform.DOScale(1, 0.4f).From(0);
            await transform.DOLocalMoveY(position.y, 0.4f).From(startPosition).AsyncWaitForCompletion();
        }

        public async UniTask RevealCardFinal()
        {
            await transform.DOScaleY(1, 0.2f).From(0).AsyncWaitForCompletion();
        }
    }
}