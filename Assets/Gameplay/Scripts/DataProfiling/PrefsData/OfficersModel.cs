using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Scripts.Utils;
using ModestTree;
using Newtonsoft.Json;
using SolidUtilities.Collections;
using Zenject;

namespace Gameplay.Scripts.DataProfiling.PrefsData
{
    public class OfficersModel : IPlayerPrefsData
    {
        private SignalBus _signalBus;

        [JsonIgnore] public List<OfficerSaveInfo> OfficerSaveInfos => _officerSaveInfos;
        [JsonProperty] private List<OfficerSaveInfo> _officerSaveInfos = new List<OfficerSaveInfo>();

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            
        }

        public OfficerSaveInfo GetSaveInfo(string key)
        {
            var officer = _officerSaveInfos.FirstOrDefault(x => x.Key == key);
            return officer;
        }

        public void AddOfficer(string key, bool withSignal = true)
        {
            if (_officerSaveInfos.Any(x => x.Key == key) == false)
            {
                var saveInfo = new OfficerSaveInfo();
                saveInfo.Key = key;
                saveInfo.Level = 1;
                saveInfo.CopiesOf = 0;
                _officerSaveInfos.Add(saveInfo);
            }
            else
            {
                var saveInfo = _officerSaveInfos.First(x => x.Key == key);
                saveInfo.CopiesOf++;
            }

            if (withSignal)
            {
                _signalBus.Fire(new OfficerGetSignal(key));
            }
        }

        public bool TrySpendCopies(string key, int count)
        {
            var saveInfo = _officerSaveInfos.FirstOrDefault(x => x.Key == key);

            if (saveInfo == null || saveInfo.CopiesOf - count < 0)
            {
                return false;
            }

            saveInfo.CopiesOf -= count;
            return true;
        }

        public void UpgradeOfficer(string key, int count)
        {
            var save = _officerSaveInfos.FirstOrDefault(x => x.Key == key);
            if (save != null)
            {
                save.Level += count;
            }
        }
    }
    

    [Serializable]
    public class OfficerSaveInfo
    {
        public string Key;
        public int Level;
        public int CopiesOf;
    }
    
}