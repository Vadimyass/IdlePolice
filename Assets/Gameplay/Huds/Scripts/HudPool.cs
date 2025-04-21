using Agents;
using Pool;
using UnityEngine;
using Zenject;

namespace Gameplay.Huds.Scripts
{
    public class HudPool : MonoBehaviour
    {
        [SerializeField] private MonoBehaviourPool<OrderHud> _orderHudPool;
        private DiContainer _container;

        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }
        public void Init(int startCount)
        {
            // for (int i = 0; i < startCount; i++)
            // {
            //     _orderHudPool.GetObject();
            // }
        }

        public OrderHud GetOrderHud()
        {
            var order = _orderHudPool.GetObject();
            _container.InjectGameObject(order.gameObject);
            return order;
        }

        public void ReturnHud(OrderHud orderHud)
        {
            _orderHudPool.ReturnObject(orderHud);
        }
        
        
    }
    
}