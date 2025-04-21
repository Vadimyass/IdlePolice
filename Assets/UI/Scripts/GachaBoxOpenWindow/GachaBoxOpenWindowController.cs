using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Audio;
using BigD.Config;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.Models;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Signals;
using Gameplay.Scripts.Utils;
using MyBox;
using Particles;
using Tutorial;
using UI.Scripts.ShopWindow;
using UI.Scripts.ShopWindow.Gacha;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace UI.Scripts.GachaBoxOpenWindow
{
    public class GachaBoxOpenWindowController : UIScreenController<GachaBoxOpenWindow>
    {
        private GachaBoxOpenWindowArguments _args;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private GameConfig _gameConfig;
        private BoxesConfigInitializator _info;
        private LocalizationManager _localizationManager;
        private SpritesConfig _spritesConfig;
        private ParticleManager _particleManager;
        private UIManager _uiManager;
        private SignalBus _signalBus;
        private AudioManager _audioManager;
        private BoxSprites _boxSprites;
        private Vector3 _boxTopStartPos;
        private SpritesConfigDictionaries _spritesConfigDictionaries;
        private bool _isClicked;


        [Inject]
        private void Construct(PlayerPrefsSaveManager playerPrefsSaveManager, AudioManager audioManager,SignalBus signalBus, UIManager uiManager, ParticleManager particleManager, SpritesConfig spritesConfig, SpritesConfigDictionaries spritesConfigDictionaries, LocalizationManager localizationManager, GameConfig gameConfig)
        {
            _spritesConfigDictionaries = spritesConfigDictionaries;
            _audioManager = audioManager;
            _signalBus = signalBus;
            _uiManager = uiManager;
            _particleManager = particleManager;
            _spritesConfig = spritesConfig;
            _localizationManager = localizationManager;
            _gameConfig = gameConfig;
            _playerPrefsSaveManager = playerPrefsSaveManager;
        }

        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            _boxTopStartPos = View.BoxTop.transform.position;
            StartAnimationBackground();
        }

        private void GetCards(Dictionary<string, int> cards)
        {
            _audioManager.PlaySound(TrackName.ButtonClick);
            if (SceneManagement.GetCurrentSceneName() == SceneManagement.SceneName.GameplayScene)
            {
                _signalBus.Fire(new RefreshShopSignal());
                _audioManager.PlayMusic(TrackName.MainMusic);
            }

            foreach (var card in cards)
            {
                for (int i = 0; i < card.Value; i++)
                {
                    _signalBus.Fire(new OfficerGetSignal(card.Key));
                }
            }
            
            _uiManager.HideLast();
            _uiManager.HideLast();
        }

        public override async UniTask Display(UIArguments arguments)
        {
            await base.Display(arguments);
            
            _audioManager.PlayMusic(TrackName.ChestOpenMusic);
            _args = (GachaBoxOpenWindowArguments)arguments;
            _info = _gameConfig.BoxesInfoConfig.GetItemByKey(_args.GachaBoxType);

            View.BoxTop.gameObject.SetActive(false);
            View.BoxTop.transform.position = _boxTopStartPos;
            View.BoxTop.transform.rotation = Quaternion.identity;
            
            _boxSprites = _spritesConfigDictionaries.GetBoxSprites(_args.GachaBoxType);
            
            View.BoxImage.sprite = _boxSprites.ClosedSprite;
            View.CloseButton.gameObject.SetActive(false);
            View.BoxNameText.text = _args.NameKey;
            
            
            View.CloseButton.onClick.RemoveAllListeners();
            View.CloseButton.onClick.AddListener(OnClick);
            
            View.ShowingAnimationCard.gameObject.SetActive(false);
            
            foreach (var resultCard in View.ResultCards)
            {
                resultCard.gameObject.SetActive(false);
                resultCard.SetInfo(null, OfficerType.CAR, OfficerRarity.Rare, 0, String.Empty);
            }
            ShowCards(_args.Count);
        }

        private void OnClick()
        {
            _isClicked = true;
        }
        
        private async void ShowCards(int count)
        {
            View.BoxImage.gameObject.SetActive(true);

            var cards = CalculateCards(count);

            await View.BoxImage.transform.DORotate(Vector3.forward * 25, 0.05f).AsyncWaitForCompletion();
            await View.BoxImage.transform.DORotate(Vector3.forward * -25, 0.1f).AsyncWaitForCompletion();
            await View.BoxImage.transform.DORotate(Vector3.forward * 25, 0.1f).AsyncWaitForCompletion();
            await View.BoxImage.transform.DORotate(Vector3.forward * -25, 0.1f).AsyncWaitForCompletion();
            await View.BoxImage.transform.DORotate(Vector3.forward * 25, 0.1f).AsyncWaitForCompletion();
            await View.BoxImage.transform.DORotate(Vector3.forward * -25, 0.1f).AsyncWaitForCompletion();
            await View.BoxImage.transform.DORotate(Vector3.forward * 25, 0.1f).AsyncWaitForCompletion();
            await View.BoxImage.transform.DORotate(Vector3.forward * -25, 0.1f).AsyncWaitForCompletion();
            await View.BoxImage.transform.DORotate(Vector3.zero, 0.05f).AsyncWaitForCompletion();
            
            View.BoxImage.sprite = _boxSprites.OpenSprite;
            View.BoxTop.sprite = _boxSprites.TopSprite;
            
            View.BoxTop.gameObject.SetActive(true);
            
            View.BoxTop.transform.DOMoveX(View.BoxTopEndPos.position.x, 0.2f).SetEase(Ease.OutCubic);
            View.BoxTop.transform.DOMoveY(View.BoxTopEndPos.position.y, 0.2f).SetEase(Ease.InCubic);
            await View.BoxTop.transform.DORotate(View.BoxTopEndPos.rotation.eulerAngles, 0.2f).AsyncWaitForCompletion();
            
            View.ShowingAnimationCard.gameObject.SetActive(true);
            
            
            int k = 0;
            
            _playerPrefsSaveManager.ForceSave();

            var dictionary = new Dictionary<string, int>();

            foreach (var card in cards)
            {
                var infoUnit = _gameConfig.OfficersInfoConfig.GetItemByKey(card);

                if (dictionary.ContainsKey(card) == false)
                {
                    dictionary.Add(card, 1);
                }
                else
                {
                    dictionary[card] += 1;
                }
                
                _playerPrefsSaveManager.PrefsData.OfficersModel.AddOfficer(card, false);
                View.ShowingAnimationCard.SetInfo(_spritesConfig.GetSpriteByName(infoUnit.SpriteName), infoUnit.OfficerType, infoUnit.OfficerRarity, 1, infoUnit.Key);

                await View.ShowingAnimationCard.RevealCard(View.ParticleTransform.localPosition, View.BoxImage.transform.localPosition);

                _audioManager.PlaySound(TrackName.CardOpenSound);

                View.CloseButton.onClick.RemoveAllListeners();
                View.CloseButton.onClick.AddListener(OnClick);
                
                View.CloseButton.gameObject.SetActive(true);
                await UniTask.WaitWhile(() => _isClicked == false);
                _isClicked = false;
                View.CloseButton.gameObject.SetActive(false);
            }

            
            
            foreach (var card in dictionary)
            {
                var infoUnit = _gameConfig.OfficersInfoConfig.GetItemByKey(card.Key);
                View.ResultCards[k++].SetInfo(_spritesConfig.GetSpriteByName(infoUnit.SpriteName), infoUnit.OfficerType, infoUnit.OfficerRarity, card.Value, infoUnit.Key);
            }
            
            View.ShowingAnimationCard.gameObject.SetActive(false);

            View.BoxImage.gameObject.SetActive(false);
            
            foreach (var card in View.ResultCards)
            {
                if (card.Count > 0)
                {
                    card.gameObject.SetActive(true);
                    await card.RevealCardFinal();
                }
            }

            await UniTask.Delay(TimeSpan.FromSeconds(1));
            
            View.CloseButton.onClick.RemoveAllListeners();
            View.CloseButton.onClick.AddListener((() => GetCards(dictionary)));
            
            View.CloseButton.gameObject.SetActive(true);
        }

        private List<string> CalculateCards(int count)
        {
            if (_playerPrefsSaveManager.PrefsData.TutorialModel.IsContextTutorDone(TutorialType.ShopTutorial) == false)
            {
                return new List<string>() { "Sheriff Gordon" };
            }
            
            var officers = _gameConfig.OfficersInfoConfig.Items;

            var rareChance = _info.RareChance/100;
            var epicChance = _info.EpicChance/100;
            var legChance = _info.LegendaryChance/100;

            var dict = new List<string>();
            
            for (int i = 0; i < count; i++)
            {
                var rarity = OfficerRarity.Rare;
                var random = Random.Range(0, 1f);
                
                
                var currentOpenCount =
                    _playerPrefsSaveManager.PrefsData.ConsumablesInfoModel.GetCountOfGachaBoxOpenedGarantee(
                        _args.GachaBoxType);
                
                if (currentOpenCount >= _info.GaranteeQuantity-1 && _info.Garantee != OfficerRarity.Rare)
                {
                    rarity = _info.Garantee;
                }
                else
                {
                    if (legChance + epicChance >= random)
                    {
                        rarity = OfficerRarity.Epic;
                        if (legChance >= random)
                        {
                            rarity = OfficerRarity.Legendary;
                        }
                    }
                    _playerPrefsSaveManager.PrefsData.ConsumablesInfoModel.IncreaseCountOfGachaBoxOpened(_args.GachaBoxType, 1);
                }

                if (_info.Garantee == rarity)
                {
                    _playerPrefsSaveManager.PrefsData.ConsumablesInfoModel.RefreshCountOfGachaBoxOpened(_args.GachaBoxType);
                }

                var officer = officers.Where(x => x.OfficerRarity == rarity).ToList().GetRandom();
                
                dict.Add(officer.Key);
            }

            return dict;
        }
        
        private void StartAnimationBackground()
        {
            View.BackgroundTransform.DOLocalMove(new Vector3(64.2f, -184.83f, 0), 3).From(new Vector3(-556.43f, 435.51f, 0)).SetLoops(-1).SetEase(Ease.Linear);
        }
    }
    
}