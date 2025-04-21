using System;
using System.Collections.Generic;
using Gameplay.Configs;
using SolidUtilities.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.OfficerInfoWidget
{
    public class OfficerInfoWidget : UIScreen
    {
        [field: SerializeField] public SerializableDictionary<OfficerRarity, Transform> RaritySprites { get; private set; }
        [field: SerializeField] public SerializableDictionary<OfficerType, TypeList> TypeSprites { get; private set; }
        [field: SerializeField] public Button UpgradeButton { get; private set; }
        [field: SerializeField] public Button CantUpgradeButton { get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Button CloseButton2 { get; private set; }
        [field: SerializeField] public TextMeshProUGUI NameText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI LevelText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI IncomeText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI IncomeChangeText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI PowerText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI PowerChangeText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI HPText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI HPChangeText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI CopiesText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI DonutCostText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI CardsCostText { get; private set; }
        [field: SerializeField] public Transform CardsCostTransform { get; private set; }
        [field: SerializeField] public List<Image> OfficerImages { get; private set; }
        [field: SerializeField] public Material DonutTextMaterial;
        [field: SerializeField] public Material NotEnoughMaterial;
    }

    [Serializable]
    public struct TypeList
    {
        public List<Transform> List;
    }
}