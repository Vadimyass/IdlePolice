using System.Collections.Generic;
using SolidUtilities.Collections;
using UI.Scripts.MainScreen;
using UI.Scripts.OfferWindow;
using UI.Scripts.ShopWindow.Gacha;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.ShopWindow
{
    public class ShopWindow : UIScreen
    {
        [field: SerializeField] public SerializableDictionary<Button, Transform> ShopSectionsAndButtons { get; private set; }
        [field: SerializeField] public SerializableDictionary<OfferType, Transform> OfferFocuses { get; private set; }
        [field: SerializeField] public List<RectTransform> Rebuildables { get; private set; }
        [field: SerializeField] public ScrollRect ScrollRect { get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }
        //[field: SerializeField] public List<ShopOfferBase> Offers { get; private set; }
        //[field: SerializeField] public List<TimeMachineOffer> TimeMachineOffers { get; private set; }
        [field: SerializeField] public List<GachaBoxView> GachaBoxOffers { get; private set; }
        [field: SerializeField] public CurrencyContainer CrystalContainer;
        [field: SerializeField] public List<CurrencyContainer> CurrencyContainers { get; private set; }
    }
}