using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.TravelWindow
{
    public class TravelWindow : UIScreen
    {
        [field: SerializeField] public Button ChoseLevelButton { get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Button BuyButton { get; private set; }
        [field: SerializeField] public Button TravelButton { get; private set; }
        [field: SerializeField] public Button UnActiveBuyButton { get; private set; }
        [field: SerializeField] public List<Sprite> Sprites { get; private set; }
        [field: SerializeField] public Image CurrentCityImage { get; private set; }
        [field: SerializeField] public Image NextCityImage { get; private set; }
        [field: SerializeField] public TextMeshProUGUI CostText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI CostText2 { get; private set; }
        
        [field: SerializeField] public TextMeshProUGUI CrystalRewardText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI BoxRewardText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI IncomeMultiplierText { get; private set; }
        
        [field: SerializeField] public TextMeshProUGUI UpgradeBuildingsText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI OpenAllZonesText { get; private set; }
        
       
    }
}