using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Scripts.CheatManager
{
    public class CheatPopupBase : CheatItemBase
    {
        [SerializeField] protected GameObject _container;
        [SerializeField] private Button _closeButton;

        private void Awake()
        {
            _closeButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}