using System;
using RotaryHeart.Lib.SerializableDictionary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.OfferWindow
{
    public class OfferWindow : UIScreen
    {
        [field: SerializeField] public TextMeshProUGUI NameText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI DescriptionText { get; private set; }
        [field: SerializeField] public SerializableDictionaryBase<OfferType, Transform> Offers { get; private set; }
        [field: SerializeField] public Button OfferButton{ get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public OfferConfig OfferConfig { get; private set; }
    }
}