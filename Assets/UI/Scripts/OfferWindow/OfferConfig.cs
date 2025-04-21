using System;
using RotaryHeart.Lib.SerializableDictionary;
using SolidUtilities.Collections;
using UnityEngine;

namespace UI.Scripts.OfferWindow
{
    public class OfferConfig : ScriptableObject
    {
         [SerializeField] private SerializableDictionaryBase<OfferType, OfferData> _offersData;

         public OfferData GetDataByType(OfferType offerType)
         {
             _offersData.TryGetValue(offerType, out var value);
             return value;
         }
    }

    [Serializable]
    public struct OfferData
    {
        public string NameKey;
        public string DescriptionKey;
        public string AnalyticKey;
    }
    
    public enum OfferType
    {
        NoAds,
        ForBeginners,
        ForExperts,
        ForPros,
        ForPatriots,
        Crystals,
        IncomeManagers,
        TimeManagers,
        
    }
}