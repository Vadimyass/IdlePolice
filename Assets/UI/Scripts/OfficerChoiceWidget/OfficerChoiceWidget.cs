using System;
using System.Collections.Generic;
using Gameplay.Configs;
using Pool;
using SolidUtilities.Collections;
using TMPro;
using UI.Scripts.OfficersWindow;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.OfficerChoiceWidget
{
    public class OfficerChoiceWidget : UIScreen
    {
        [field: SerializeField] public MonoBehaviourPool<OfficerChoiceView> OfficersChoicePool { get; private set; }
        [field: SerializeField] public Transform EmptyOfficerInfo { get; private set; }
        [field: SerializeField] public Transform ChosenOfficerInfo { get; private set; }
        [field: SerializeField] public OfficerCard OfficerCard { get; private set; }
        [field: SerializeField] public List<TextMeshProUGUI> LvlNeedTexts { get; private set; }
        [field: SerializeField] public OfficerChoiceListsButton SuitableButton { get; private set; }
        [field: SerializeField] public OfficerChoiceListsButton NotSuitableButton { get; private set; }
        [field: SerializeField] public Button ShopButton { get; private set; }
        [field: SerializeField] public Transform NoOfficersTransform { get; private set; }
        [field: SerializeField] public TextMeshProUGUI BuildingName { get; private set; }
        [field: SerializeField] public SerializableDictionary<OfficerType, ListForAgentType> ItemsByTypeForBuilding { get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Button RecallButton { get; private set; }
        [field: SerializeField] public List<RectTransform> RebuildTransforms { get; private set; }
    }

    [Serializable]
    public struct ListForAgentType
    {
        public List<Transform> Transforms;
    }
}