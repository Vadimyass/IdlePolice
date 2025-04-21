using UnityEngine;

namespace UI.Huds.Scripts
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField]
        private Transform Target;
        [SerializeField]
        private Vector3 Offset;

        private void Update()
        {
            transform.position = Target.position + Offset;
        }
    }
}