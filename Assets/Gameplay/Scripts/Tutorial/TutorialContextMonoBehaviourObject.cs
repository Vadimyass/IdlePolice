using UnityEngine;

namespace Gameplay.Scripts.Tutorial
{
    public abstract class TutorialContextMonoBehaviourObject : MonoBehaviour , ITutorialContextObject
    {
        public virtual void Activate()
        {
            gameObject.SetActive(true);
        }

        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}