using System;
using Agents;
using Cysharp.Threading.Tasks;
using Gameplay.OrderManaging;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Particles;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using IPoolable = Pool.IPoolable;

namespace Gameplay.Huds.Scripts
{
    public class OrderHud : MonoBehaviour , IPoolable
    {
        [SerializeField] private CrimesHud _crimes;
        [SerializeField] private ArrestHud _arrest;
        [SerializeField] private DollarHud _dollar;
        [SerializeField] private Image _logoBlue;

        public CrimesHud CrimesHud => _crimes;
        public ArrestHud ArrestHud => _arrest;
        public DollarHud DollarHud => _dollar;
        
        private SpritesConfig _spritesConfig;
        private LevelController _levelController;
        private ParticleManager _particleManager;
        private UIManager _uiManager;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private CarAgent _carAgent;
        private IOrder _order;
        private OrderManager _orderManager;


        [Inject]
        private void Construct(SpritesConfig spritesConfig, UIManager uiManager, ParticleManager particleManager, LevelController levelController,PlayerPrefsSaveManager playerPrefsSaveManager,OrderManager orderManager)
        {
            _orderManager = orderManager;
            _uiManager = uiManager;
            _particleManager = particleManager;
            _playerPrefsSaveManager = playerPrefsSaveManager;
            _levelController = levelController;
            _spritesConfig = spritesConfig;
        }

        public void Init(IOrder order)
        {
            _order = order;
            
            _dollar.Init(() =>
            {
                _levelController.CurrentLevel.PointsContainer.ReturnHud(this);
            });
            
            _arrest.Init(() =>
            {
                _levelController.CurrentLevel.PointsContainer.ReturnHud(this);
            });
            
            _crimes.Init(async () =>
            {
                _order.CompleteOrder();

                await UniTask.Delay(TimeSpan.FromSeconds(1.2f));
                _levelController.CurrentLevel.PointsContainer.ReturnHud(this);
            });
        }
        public void SetPosition(Vector3 position)
        {
            transform.position = new Vector3(position.x,transform.position.y,position.z);
            ChangeHud(OrderType.Crime);
        }
        

        public void ChangeHud(OrderType pointType)
        {
            _crimes.SetActive(pointType == OrderType.Crime);
            _arrest.SetActive(pointType == OrderType.Arrest);
            _dollar.SetActive(pointType == OrderType.Dollar);
        }

        public void SetSprite(SpriteName spriteName)
        {
            _logoBlue.sprite = _spritesConfig.GetSpriteByName(spriteName);
        }

        public void Return()
        {
            gameObject.SetActive(false);
        }

        public void Release()
        {
            gameObject.SetActive(false);
        }
    }

    public enum OrderType
    {
        Crime,
        Arrest,
        Dollar,
    }
}