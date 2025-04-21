using System;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Button = UnityEngine.UI.Button;

namespace UI.UIUtils
{
    [RequireComponent(typeof(AsyncPointerDownTrigger))]
    public class ExtendedButton : Button
    {
        [Serializable]
        public class OnPointerDownEvent : UnityEvent<PointerEventData> {}

        // Event delegates triggered on click.
        [SerializeField]
        private OnPointerDownEvent _onPointerDown = new OnPointerDownEvent();

        public OnPointerDownEvent OnPointerDown => _onPointerDown;
        
        protected override void Start()
        {
            base.Start();
            gameObject.GetAsyncPointerDownTrigger().Subscribe(x => _onPointerDown.Invoke(x));
        }
    }
}