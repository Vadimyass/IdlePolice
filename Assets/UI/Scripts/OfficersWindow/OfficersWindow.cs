using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.OfficersWindow
{
    public class OfficersWindow : UIScreen
    {
        [field: SerializeField] public TextMeshProUGUI OwnText { get; private set; }
        [field: SerializeField] public MonoBehaviourPool<OfficerCard> FoundOfficers { get; private set; }
        [field: SerializeField] public MonoBehaviourPool<OfficerCard> LockedOfficers { get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public List<RectTransform> LayoutRectTransform;
    }
}