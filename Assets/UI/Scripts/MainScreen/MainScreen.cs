using System;
using System.Collections.Generic;
using Pool;
using SolidUtilities.Collections;
using Tutorial;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.MainScreen
{
    public class MainScreen : UIScreen
    {
        [field: SerializeField] public List<CurrencyContainer> CurrencyContainers { get; private set; }
        [field: SerializeField] public Button UpgradesButton { get; private set; }
        [field: SerializeField] public Button OfficersButton { get; private set; }
        [field: SerializeField] public Button ChoseLevelButton { get; private set; }
        [field: SerializeField] public Button OpenShopButton { get; private set; }
        [field: SerializeField] public MilestoneSlider MilestoneSlider { get; private set; }
        [field: SerializeField] public List<MainScreenWidgetBlock> WidgetBlocks { get; private set; }
        [field: SerializeField] public SerializableDictionary<TutorialType, TransformsList> HideBeforeTutor { get; private set; }
        [field: SerializeField] public MonoBehaviourPool<SmallOfficerCard> SmallOfficerCardPool { get; private set; }
        [field: SerializeField] public MonoBehaviourPool<GachaBoxSmallImage> GachaBoxSmallPool { get; private set; }
    }
    
    [Serializable]
    public struct TransformsList
    {
        public List<CanvasGroup> List;
    }
}