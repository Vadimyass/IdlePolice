using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Scripts.CheatManager
{
    public class CheatWindow : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private Button _closeButton;

        public Transform Content => _content;
        private RectTransform RectTransform => (RectTransform) _content;

        public void Awake()
        {
            _closeButton.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}