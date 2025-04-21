using UnityEngine;

namespace Particles
{
    public class ParticleAutoLayer : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        void Start ()
        {
            //Change Foreground to the layer you want it to display on 
            //You could prob. make a public variable for this
            _particleSystem.GetComponent<Renderer>().sortingLayerName = "Foreground";
        }
    }
}