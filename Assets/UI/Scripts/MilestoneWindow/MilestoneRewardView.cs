using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.MilestoneWindow
{
    public class MilestoneRewardView : MonoBehaviour
    {
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private Transform _completeTransform;

        public void Show(Sprite sprite, string rewardText, bool isComplete)
        {
            _rewardImage.sprite = sprite;
            _rewardText.text = rewardText;
            _completeTransform.gameObject.SetActive(isComplete);
        }
    }
}