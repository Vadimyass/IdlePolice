using Gameplay.Scripts.Utils;
using UnityEngine;

namespace UI.Huds.Scripts
{
    public abstract class HudContainer : MonoBehaviour
    {
        [SerializeField] protected Vector3 _offset;

        private Vector3 _defaultOffset;

        protected IHudOwner _hudOwner;
        
        public void Init(IHudOwner fortAgent)
        {
            _hudOwner = fortAgent;
            _defaultOffset = _offset;
            foreach (var reflection in ReflectionUtils.GetFieldsOfType<HudBase>(this))
            {
                reflection.Init();
            }
        }

        public void SetOffset(Vector3 newOffset)
        {
            _offset = newOffset;
        }

        public void SetDefaultOffset()
        {
            _offset = _defaultOffset;
        }
        
        private void Update()
        {
            if(_hudOwner == null || _hudOwner.MainTransform == null) return;
            
            transform.position = _hudOwner.MainTransform.position + _offset;
        }
    }
}