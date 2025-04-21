using System.Collections.Generic;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling.PrefsData;
using SolidUtilities.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts.GachaBoxOpenWindow
{
    public class GachaCardWithInfo : GachaCard
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private SerializableDictionary<OfficerRarity, Transform> _rarityTexts;
        [SerializeField] private Image _officerImage;
        [SerializeField] private TextMeshProUGUI _officerCountCurrent;
        [SerializeField] private ParticleSystem _shiningParticle;
        private PlayerPrefsSaveManager _prefsSaveManager;

        [Inject]
        private void Construct(PlayerPrefsSaveManager prefsSaveManager)
        {
            _prefsSaveManager = prefsSaveManager;
        }
        
        public override void SetInfo(Sprite mainSprite, OfficerType officerType, OfficerRarity officerRarity, int count, string nameKey)
        {
            base.SetInfo(mainSprite, officerType, officerRarity, count, nameKey);
            _nameText.text = nameKey;
            
            foreach (var spritesType in _rarityTexts)
            {
                spritesType.Value.gameObject.SetActive(false);
            }
            
            _rarityTexts[officerRarity].gameObject.SetActive(true);

            _officerImage.sprite = mainSprite;
            _officerCountCurrent.text = (_prefsSaveManager.PrefsData.OfficersModel.GetSaveInfo(nameKey).CopiesOf + count).ToString();
            _shiningParticle.Play();
        }
    }
}