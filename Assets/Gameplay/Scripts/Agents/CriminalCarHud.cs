using UnityEngine;

namespace Gameplay.Scripts.Agents
{
    public class CriminalCarHud : MonoBehaviour
    {
        [SerializeField] private Transform _image;

        public void ShowImage()
        {
            _image.gameObject.SetActive(true);
        }

        public void HideImage()
        {
            _image.gameObject.SetActive(false);
        }
    }
}