using UnityEngine;

namespace UI.Huds.Scripts
{
    public interface IHudOwner
    {
        public Transform MainTransform { get; set; }
    }
}