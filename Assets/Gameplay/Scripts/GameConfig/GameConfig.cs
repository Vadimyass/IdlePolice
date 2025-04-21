

using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Utils;
using MyBox;
using SolidUtilities.Collections;
using TypeReferences;
using UI.Scripts.DialogWindow;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace BigD.Config
{
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private string _sheetId;
        [SerializeField] private List<Sheet> _sheets;
        
        public UpgradeConfig UpgradeConfig = new ();
        public UpgradeBaseConfig UpgradeBaseConfig = new ();
        public LocalizationConfig LocalizationConfig = new ();
        public OfficersLevelsInfoConfig OfficersLevelsInfoConfig = new ();
        public OfficersInfoConfig OfficersInfoConfig = new ();
        public CriminalsConfig CriminalsConfig = new ();
        public EconomyConfig EconomyConfig = new();
        public MissionsConfig MissionsConfig = new();
        public MilestonesConfig MilestonesConfig = new();
        public DotsUnlockConfig DotsUnlockConfig = new();
        public BoxesInfoConfig BoxesInfoConfig = new();
        public DialogueConfig DialogueConfig = new();
        
        [SerializeField] private List<CriminalInitializator> _items = new ();
        
        public async UniTask LoadConfigs(DiContainer diContainer)
        {
            var reflections = ReflectionUtils.GetFieldsOfType<IConfig>(this);
            foreach (var sheet in _sheets)
            {
                var instanceConfig = reflections.First(x => x.GetType() == sheet.Config.Type);
                instanceConfig.LoadConfig(sheet.Name);
                diContainer.Inject(instanceConfig);
            }
        }

#if UNITY_EDITOR
        [ButtonMethod()]
        public async void RefreshAll()
        {
            foreach (var sheet in _sheets)
            {
                var request = UnityWebRequest.Get("https://docs.google.com/spreadsheets/d/e/" + _sheetId + "/pub?output=csv" + "&gid=" + sheet.GID);
                await request.SendWebRequest();
                var csvString = request.downloadHandler.text;
                IParser parser = (IParser)Activator.CreateInstance(sheet.Parser.Type);
                var listPhrases = parser.ParseObject(csvString);
                DataController.SaveSheetIntoJson(listPhrases,sheet.Name);
                if (sheet.Name == "Criminals")
                {
                    _items = (List<CriminalInitializator>)listPhrases;
                }
            }

            Debug.LogError("all sheets loaded successfully!");
        }
#endif
    }

    [Serializable]
    public class Sheet
    {
        public string Name;
        public string GID;
        [Inherits(typeof(IParser))]
        public TypeReference Parser;
        
        [Inherits(typeof(IConfig))]
        public TypeReference Config;
        
        public TextAsset OverridedCsv;
    }

}
