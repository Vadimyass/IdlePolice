using System.Collections.Generic;
using Agents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.LoadingScreen
{
    public class LoadingScreen : UIScreen
    {
        [field: SerializeField] public List<Transform> CarAgents { get; private set; }
        [field: SerializeField] public List<Transform> StartPoints { get; private set; }
        [field: SerializeField] public List<Transform> EndPoints { get; private set; }
        [field: SerializeField] public TextMeshProUGUI TextMeshProUGUI { get; private set; }
        [field: SerializeField] public Slider ProgressSlider { get; private set; }
    }
}