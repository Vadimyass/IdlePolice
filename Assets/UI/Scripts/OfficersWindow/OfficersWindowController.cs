using System.Collections.Generic;
using System.Linq;
using Audio;
using BigD.Config;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling.PrefsData;
using UI.Scripts.OfficerChoiceWidget;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts.OfficersWindow
{
    public class OfficersWindowController : UIScreenController<OfficersWindow>
    {
        private PlayerPrefsSaveManager _prefsSaveManager;
        private GameConfig _gameConfig;
        private SpritesConfig _spritesConfig;
        private UIManager _uiManager;

        [Inject]
        private void Construct(GameConfig gameConfig, UIManager uiManager, PlayerPrefsSaveManager prefsSaveManager, SpritesConfig spritesConfig)
        {
            _uiManager = uiManager;
            _spritesConfig = spritesConfig;
            _gameConfig = gameConfig;
            _prefsSaveManager = prefsSaveManager;
        }

        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            var officers = _gameConfig.OfficersInfoConfig.Items;
            
            foreach (var officer in officers)
            {
                var card = View.FoundOfficers.GetObject();
                card.Init(_spritesConfig, _prefsSaveManager, _uiManager);
                var card2 = View.LockedOfficers.GetObject();
                card2.Init(_spritesConfig, _prefsSaveManager, _uiManager);
            }
        }

        public override async UniTask Display(UIArguments arguments)
        {
            await base.Display(arguments);
            
            View.CloseButton.onClick.RemoveAllListeners();
            View.CloseButton.onClick.AddListener((() =>
            {
                _audioManager.PlaySound(TrackName.ButtonClick);
                _uiManager.HideLast();
            }));
            
            View.FoundOfficers.ReturnAll();
            View.LockedOfficers.ReturnAll();

            var officers = _gameConfig.OfficersInfoConfig.Items.ToList();

            var saveInfo = _prefsSaveManager.PrefsData.OfficersModel.OfficerSaveInfos.ToList();;
            var getOfficers = new List<OfficerConfigInitializator>();

            var infos = new List<OfficerInfo>();
            
            
            foreach (var officerSaveInfo in saveInfo)
            {
                var info = _gameConfig.OfficersInfoConfig.GetItemByKey(officerSaveInfo.Key);
                var levelInfo = _gameConfig.OfficersLevelsInfoConfig.GetItemByKey(info.OfficerRarity, officerSaveInfo.Level);
                
                var fullInfo = new OfficerInfo();
                fullInfo.Info = info;
                fullInfo.LevelInfo = levelInfo;
                fullInfo.SaveInfo = officerSaveInfo;
                
                infos.Add(fullInfo);
            }

            infos = infos.OrderByDescending(o => o.LevelInfo.IncomeMultiplier).ToList();
            
            for (int i = 0; i < officers.Count; i++)
            {
                if (saveInfo.Any(x => x.Key == officers[i].Key))
                {
                    getOfficers.Add(officers[i]);
                }
            }
            
            View.OwnText.text = getOfficers.Count + "/" + officers.Count;
            
            
            
            foreach (var officer in infos)
            {
                officers.Remove(officer.Info);
                var card = View.FoundOfficers.GetObject();
                card.Show(officer);
            }

            foreach (var officer in officers)
            {
                var card = View.LockedOfficers.GetObject();
                card.Show(officer);
            }

            

            foreach (var rectTransform in View.LayoutRectTransform)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }
            
        }
    }
}