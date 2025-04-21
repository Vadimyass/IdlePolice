using System.Collections.Generic;
using Gameplay.Configs;
using SolidUtilities.Collections;
using TMPro;
using UI.Scripts.OfficerInfoWidget;
using UI.Scripts.OfficersWindow;
using UI.UIUtils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.UpgradeBuildingWindow
{
    public class UpgradeBuildingWindow : UIScreen
    {
        [field: SerializeField] public List<Transform> HideOnMaxLevel { get; private set; }
        [field: SerializeField] public List<Transform> ShowOnMaxLevel { get; private set; }
        [field: SerializeField] public SerializableDictionary<OfficerType, Transform> TypeSprites { get; private set; }
        [field: SerializeField] public SerializableDictionary<OfficerType, Transform> TypeSpritesOfficer { get; private set; }
        [field: SerializeField] public Image ItemImage { get; private set; }
        [field: SerializeField] public OfficerCard OfficerCard { get; private set; }
        [field: SerializeField] public Button ChoseCapoButton { get; private set; }
        [field: SerializeField] public Button UpgradeButton { get; private set; }
        [field: SerializeField] public Button NotEnoughButton { get; private set; }
        [field: SerializeField] public Button SwitchUpgradeButton { get; private set; }
        [field: SerializeField] public Image SingleModeImage { get; private set; }
        [field: SerializeField] public Image MaxModeImage { get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Button CloseButton2 { get; private set; }
        [field: SerializeField] public Button NextBuildingButton { get; private set; }
        [field: SerializeField] public Button PreviousBuildingButton { get; private set; }
        [field: SerializeField] public Slider ProgressSlider { get; private set; }
        [field: SerializeField] public Slider FutureProgressSlider { get; private set; }
        [field: SerializeField] public TextMeshProUGUI IncomeText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI FutureIncomeText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI MultiplierText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI FutureLevelsText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI LevelText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI NameText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ItemsNameText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI DurationText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI AutomaticText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI CostText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI CostText2 { get; private set; }
        [field: SerializeField] public TextMeshProUGUI CarsText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI CarsAddText { get; private set; }
        [field: SerializeField] public RectTransform HorizontalGroup { get; private set; }
        [field: SerializeField] public RectTransform LevelHorizontalGroup { get; private set; }
        [field: SerializeField] public RectTransform NameHorizontalGroup { get; private set; }
        [field: SerializeField] public TextMeshProUGUI X1Text { get; private set; }
        [field: SerializeField] public TextMeshProUGUI MaxText { get; private set; }
    }
}