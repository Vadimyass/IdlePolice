using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.DialogWindow
{
    public class DialogWindow : UIScreen
    {
        [field: SerializeField] public TextMeshProUGUI PhraseText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI SpeakerNameText { get; private set; }
        [field: SerializeField] public Image SpeakerImage { get; private set; }
        [field: SerializeField] public Button SkipStepButton { get; private set; }
        [field: SerializeField] public Button TutorialSkipButton { get; private set; }
    }
}