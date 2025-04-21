using UnityEngine;

namespace Gameplay.Scripts.Tutorial
{
    public class TutorialWorldSpaceTarget : TutorialTarget
    {
        public override Vector2 CalculateSize()
        {
            return new Vector2(100, 100);
        }

        public override Vector3 CalculatePosition()
        {
            return Camera.main.WorldToScreenPoint(transform.position);
        }
    }
}