using System;
using Audio;
using BigD.Config;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Utils;
using UI.UIUtils;
using UnityEngine;
using Zenject;

namespace UI.Scripts.OfficerInfoWidget
{
    public class OfficerInfoWidgetController : UIScreenController<OfficerInfoWidget>
    {
        private PlayerPrefsSaveManager _prefsSaveManager;
        private GameConfig _gameConfig;
        private OfficerConfigInitializator _officer;
        private SpritesConfig _spritesConfig;
        private UIManager _uiManager;

        [Inject]
        private void Construct(GameConfig gameConfig, SpritesConfig spritesConfig, PlayerPrefsSaveManager prefsSaveManager, UIManager uiManager)
        {
            _uiManager = uiManager;
            _spritesConfig = spritesConfig;
            _gameConfig = gameConfig;
            _prefsSaveManager = prefsSaveManager;
        }
        
        public override async UniTask Display(UIArguments arguments)
        {
            var args = (OfficerInfoWidgetArguments)arguments;

            View.CloseButton.onClick.RemoveAllListeners();
            View.CloseButton.onClick.AddListener((() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.HideLast();
            }));
            
            View.CloseButton2.onClick.RemoveAllListeners();
            View.CloseButton2.onClick.AddListener((() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.HideLast();
            }));
            
            View.UpgradeButton.onClick.RemoveAllListeners();
            View.UpgradeButton.onClick.AddListener(Upgrade);

            _officer = _gameConfig.OfficersInfoConfig.GetItemByKey(args.OfficerKey);

            View.NameText.text = _officer.Key;

            foreach (var officerImage in View.OfficerImages)
            {
                officerImage.sprite = _spritesConfig.GetSpriteByName(_officer.SpriteName);
            }
            
            foreach (var viewTypeSprite in View.TypeSprites)
            {
                foreach (var transform in viewTypeSprite.Value.List)
                {
                    transform.gameObject.SetActive(false);
                }
            }

            foreach (var transform in View.RaritySprites)
            {
                transform.Value.gameObject.SetActive(false);
            }
            
            foreach (var transform in View.TypeSprites[_officer.OfficerType].List)
            {
                transform.gameObject.SetActive(true);
            }

            View.RaritySprites[_officer.OfficerRarity].gameObject.SetActive(true);

            RefreshInfo();
            await base.Display(arguments);
        }

        private void RefreshInfo()
        {
            var level = _prefsSaveManager.PrefsData.OfficersModel.GetSaveInfo(_officer.Key).Level;
            var currentInfo = _gameConfig.OfficersLevelsInfoConfig.GetItemByKey(_officer.OfficerRarity, level);
            View.LevelText.text = "<color="+"yellow"+">Lv.</color>" + currentInfo.Level;
            
            View.IncomeText.text = TextMeshProUtils.ConvertBigDoubleToText(currentInfo.IncomeMultiplier);
            View.PowerText.text = TextMeshProUtils.ConvertBigDoubleToText(currentInfo.FightPower);
            View.HPText.text = TextMeshProUtils.ConvertBigDoubleToText(currentInfo.HP);
            View.CopiesText.text = _prefsSaveManager.PrefsData.OfficersModel.GetSaveInfo(_officer.Key).CopiesOf.ToString();
            
            View.CardsCostTransform.gameObject.SetActive(false);
            
            var nextLevelInfo = _gameConfig.OfficersLevelsInfoConfig.GetItemByKey(_officer.OfficerRarity, level+1);
            
            if (nextLevelInfo == null)
            {
                View.CantUpgradeButton.gameObject.SetActive(false);
                View.UpgradeButton.gameObject.SetActive(false);
                View.IncomeChangeText.text = "";
                View.PowerChangeText.text = "";
                View.HPChangeText.text = "";
                return;
            }
            
            View.IncomeChangeText.text = "+" + TextMeshProUtils.ConvertBigDoubleToText(nextLevelInfo.IncomeMultiplier - currentInfo.IncomeMultiplier);
            View.PowerChangeText.text = "+" + TextMeshProUtils.ConvertBigDoubleToText(nextLevelInfo.FightPower - currentInfo.FightPower);
            View.HPChangeText.text = "+" + TextMeshProUtils.ConvertBigDoubleToText(nextLevelInfo.HP - currentInfo.HP);
            
            var saveInfo = _prefsSaveManager.PrefsData.OfficersModel.GetSaveInfo(_officer.Key);
            
            if (_prefsSaveManager.PrefsData.CurrenciesModel.DonutCount < currentInfo.DonutCost ||
                saveInfo.CopiesOf < currentInfo.UpgradeCardCost)
            {
                View.CantUpgradeButton.gameObject.SetActive(true);
                View.UpgradeButton.gameObject.SetActive(false);
                View.DonutCostText.material = View.NotEnoughMaterial;
                View.DonutCostText.color = Color.red;
                View.CardsCostText.color = Color.red;
            }
            else
            {
                View.CantUpgradeButton.gameObject.SetActive(false);
                View.UpgradeButton.gameObject.SetActive(true);
                View.DonutCostText.material = View.DonutTextMaterial;
                View.DonutCostText.color = Color.white;
                View.CardsCostText.color = Color.green;
            }

            View.DonutCostText.text = TextMeshProUtils.ConvertBigDoubleToText(currentInfo.DonutCost);
            if (currentInfo.UpgradeCardCost > 0)
            {
                View.CardsCostText.text = saveInfo.CopiesOf + "/" + currentInfo.UpgradeCardCost;
                View.CardsCostTransform.gameObject.SetActive(true);
            }
        }
        
        private void Upgrade()
        {
            var saveInfo = _prefsSaveManager.PrefsData.OfficersModel.GetSaveInfo(_officer.Key);
            var info = _gameConfig.OfficersLevelsInfoConfig.GetItemByKey(_officer.OfficerRarity, saveInfo.Level);
            
            if(_prefsSaveManager.PrefsData.CurrenciesModel.DonutCount < info.DonutCost || saveInfo.CopiesOf < info.UpgradeCardCost) return;
            
            if (_prefsSaveManager.PrefsData.CurrenciesModel.TrySpendDonut(info.DonutCost) && _prefsSaveManager.PrefsData.OfficersModel.TrySpendCopies(_officer.Key, info.UpgradeCardCost))
            {
                _prefsSaveManager.PrefsData.OfficersModel.UpgradeOfficer(_officer.Key, 1);
                _audioManager.PlaySound(TrackName.UpgradeButton);
                UpgradeEffect(View.UpgradeButton.transform);
                UpgradeEffect(View.LevelText.transform);
                UpgradeEffect(View.IncomeText.transform);
                UpgradeEffect(View.PowerText.transform);
                UpgradeEffect(View.HPText.transform);
                UpgradeEffect(View.OfficerImages[0].transform);
                RefreshInfo();
            }
        }

        private void UpgradeEffect(Transform transform)
        {
            var seq = DOTween.Sequence();
            seq.Append(transform.DOScale(1.1f, 0.1f));
            seq.Append(transform.DOScale(1, 0.1f));
        }
        
        public override UniTask OnHide()
        {
            View.CloseButton.onClick.RemoveAllListeners();
            View.CloseButton2.onClick.RemoveAllListeners();
            View.UpgradeButton.onClick.RemoveAllListeners();
            return base.OnHide();
        }
    }
}