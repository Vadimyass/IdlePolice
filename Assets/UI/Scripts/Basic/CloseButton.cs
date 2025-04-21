using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class CloseButton : MonoBehaviour
    {
        [SerializeField] private UIType _uiType;
        private Button _selfButton;
        private UIManager _uiManager;

        [Inject]
        private void Construct(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
        private void Awake()
        {
            _selfButton = GetComponent<Button>();
            _selfButton.onClick.AddListener(() => _uiManager.HideLast());
        }

        private void OnDestroy()
        {
            _selfButton.onClick.RemoveAllListeners();
        }
    }
}