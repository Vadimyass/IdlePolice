using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.GachaBoxOpenWindow
{
    public class GachaBoxOpenWindow : UIScreen
    {
        [field: SerializeField] public TextMeshProUGUI BoxNameText { get; private set; }
        [field: SerializeField] public GachaCardWithInfo ShowingAnimationCard { get; private set; }
        [field: SerializeField] public List<GachaCard> ResultCards { get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public Transform ParticleTransform { get; private set; }
        [field: SerializeField] public Transform BackgroundTransform { get; private set; }
        [field: SerializeField] public Image BoxImage { get; private set; }
        [field: SerializeField] public Image BoxTop { get; private set; }
        [field: SerializeField] public Transform BoxTopEndPos { get; private set; }
    }
}