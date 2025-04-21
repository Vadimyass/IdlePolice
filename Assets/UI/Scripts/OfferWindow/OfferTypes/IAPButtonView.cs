using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Scripts.IAPController
{
    public class IAPButtonView : MonoBehaviour
    {
        /*[SerializeField] private Button _buyButton;
        [SerializeField] private TextMeshProUGUI _currencyText;
        [SerializeField] private GameObject _purchasedImage;
        [SerializeField] private string _productId;
        protected TextMeshProUGUI CurrencyText => _currencyText;

        protected Product _product;
        protected SignalBus _signalBus;
        protected PlayerPrefsSaveManager _playerPrefsSaveManager;
        protected MilitaryBaseController _militaryBaseController;
        protected IncomeController _incomeController;
        protected IAPManager _iapManager;


        [Inject]
        private void Construct(SignalBus signalBus,PlayerPrefsSaveManager playerPrefsSaveManager,MilitaryBaseController militaryBaseController,IncomeController incomeController,IAPManager iapManager)
        {
            _iapManager = iapManager;
            _incomeController = incomeController;
            _militaryBaseController = militaryBaseController;
            _playerPrefsSaveManager = playerPrefsSaveManager;
            _signalBus = signalBus;
        }

        public async virtual void Init()
        {
            _buyButton.onClick.RemoveAllListeners();
            _signalBus.TryUnsubscribe<OnSuccessfullPurchaseSignal>(TryValidate);
            _signalBus.TryUnsubscribe<RefreshShopSignal>(Refresh);
            
            _product = await _iapManager.GetProductById(_productId);
            
            if (CheckForPurchasingProduct(true) == false)
            {
                _currencyText.text = _product.metadata.localizedPriceString;
            }


            Refresh();

            _signalBus.Subscribe<RefreshShopSignal>(Refresh);
            
            
            _buyButton.onClick.AddListener(() =>
            {
                _iapManager.BuyProductID(_productId);
            });
            
            _signalBus.Subscribe<OnSuccessfullPurchaseSignal>(product =>
            {
                TryValidate(product);
            });
            


        }

        private void TryValidate(OnSuccessfullPurchaseSignal product)
        {
            if (product.Product.definition.id.Equals(_product.definition.id,StringComparison.Ordinal))
            {
                Debug.LogError("on validate");
                CheckForPurchasingProduct(false);
            }
        }

        public void Dispose()
        {
            _buyButton.onClick.RemoveAllListeners();
            _signalBus.TryUnsubscribe<OnSuccessfullPurchaseSignal>(TryValidate);
            _signalBus.TryUnsubscribe<RefreshShopSignal>(Refresh);
        }

        private bool CheckForPurchasingProduct(bool isFromStart)
        {
            if (IsPurchasedProduct() && _product.definition.type != ProductType.Consumable)
            {
                OnPurchase(isFromStart);
                ShowPurchasedImage();
                return true;
            }

            return false;
        }

        public virtual void Refresh()
        {
            
        }

        public virtual void OnPurchase(bool isFromStart)
        {
            
        }
#if BIGD_TEST_UNITY_INAPP
        private bool IsPurchasedProduct()
        {
            return false;
        }
#else
        private bool IsPurchasedProduct()
        {
            return _product.hasReceipt;
        }
#endif


        protected void ShowPurchasedImage()
        {
            _purchasedImage.SetActive(true);
        }

        protected void SendAnalytics()
        {
#if !BIGD_TEST_UNITY_INAPP
                        AnalyticsManager.LogAppMetricaEvent(EventMetricaName.payment_succeed,false,
                (EventMetricaParameterName.inapp_id,_product.definition.id),
                (EventMetricaParameterName.currency, _product.metadata.isoCurrencyCode),
                (EventMetricaParameterName.price,(float)_product.metadata.localizedPrice));
#endif
        }
        */
        
    }
}