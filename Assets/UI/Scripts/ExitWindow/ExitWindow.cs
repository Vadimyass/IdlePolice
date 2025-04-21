using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Scripts.ExitWindow
{
    public class ExitWindow : UIScreen
    {
        [field: SerializeField] public Button AcceptButton { get; private set; }
        [field: SerializeField] public Button DeclineButton { get; private set; }
    }
}