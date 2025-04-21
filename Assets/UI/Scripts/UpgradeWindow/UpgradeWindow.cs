using Pool;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.UpgradeWindow
{
    public class UpgradeWindow : UIScreen
    {
        [field: SerializeField] public MonoBehaviourPool<UpgradeView> UpgradeViewPool { get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }
    }
}