using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.GachaBoxWidget
{
    public class GachaBoxWidget : UIScreen
    {
        [field: SerializeField] public Image BoxImage { get; private set; }
        [field: SerializeField] public TextMeshProUGUI NameText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI BasicCardsText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI SilverCardsText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI GolderCardsText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI AvailableBoxText { get; private set; }
        [field: SerializeField] public Button OpenButton { get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }
    }
}