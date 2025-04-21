using System.Collections.Generic;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Newtonsoft.Json;
using UI.Scripts.ShopWindow;
using UI.Scripts.ShopWindow.Gacha;
using Zenject;

namespace Gameplay.Scripts.DataProfiling.Models
{
    public class СonsumablesInfoModel : IPlayerPrefsData
    {
        private SignalBus _signalBus;
        /*[JsonProperty]
        public Dictionary<TimeMachineType, Dictionary<TimeMachineTime, int>> TimeMachines { get; private set; }
            = new Dictionary<TimeMachineType, Dictionary<TimeMachineTime, int>>()
            {
                {TimeMachineType.Money, new Dictionary<TimeMachineTime, int>(){ { TimeMachineTime.oneHour,0}, { TimeMachineTime.fourHours,0},{ TimeMachineTime.twentyFourHours,0}}},
                {TimeMachineType.Soldiers, new Dictionary<TimeMachineTime, int>(){ { TimeMachineTime.oneHour,0}, { TimeMachineTime.fourHours,0},{ TimeMachineTime.twentyFourHours,0}}},
            };
            */

        [JsonProperty]
        public Dictionary<GachaBoxType, int> GachaBoxes { get; private set; } = new Dictionary<GachaBoxType, int>();

        [JsonProperty]
        public Dictionary<GachaBoxType, int> OpenedBoxCountWithoutGarantee { get; private set; } = new Dictionary<GachaBoxType, int>();

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            
        }

        /*public void AddTimeMachine(TimeMachineType timeMachineType, TimeMachineTime timeMachineTime, int count)
        {
            TimeMachines.TryGetValue(timeMachineType, out var timeMachines);
            timeMachines[timeMachineTime] += count;
        }

        public bool TryConsumeTimeMachine(TimeMachineType timeMachineType, TimeMachineTime timeMachineTime)
        {
            TimeMachines.TryGetValue(timeMachineType, out var timeMachines);
            if (timeMachines[timeMachineTime] > 0)
            {
                timeMachines[timeMachineTime] -= 1;
                return true;
            }

            return false;
        }

        public int GetCountOfTimeMachine(TimeMachineType timeMachineType, TimeMachineTime timeMachineTime)
        {
            TimeMachines.TryGetValue(timeMachineType, out var timeMachines);
            return timeMachines[timeMachineTime];
        }
        */
        
        public void AddGachaBox(GachaBoxType gachaBoxType, int count)
        {
            if(GachaBoxes.TryGetValue(gachaBoxType, out var gachaBoxes))
            {
                GachaBoxes[gachaBoxType] += count;
                _signalBus.Fire(new GachaBoxGetSignal(gachaBoxType));    
                return;
            }
            
            GachaBoxes.Add(gachaBoxType, count);
            _signalBus.Fire(new GachaBoxGetSignal(gachaBoxType));  
        }
        
        public bool TryConsumeGachaBox(GachaBoxType gachaBoxType, int count)
        {
            GachaBoxes.TryGetValue(gachaBoxType, out var gachaBoxes);
            if (gachaBoxes >= count)
            {
                GachaBoxes[gachaBoxType] -= count;
                return true;
            }

            return false;
        }
        
        public int GetCountOfGachaBox(GachaBoxType gachaBoxType)
        {
            if (GachaBoxes.TryGetValue(gachaBoxType, out var gachaBoxes))
            {
                return gachaBoxes; 
            }

            return 0;
        }
        
        public int GetCountOfGachaBoxOpenedGarantee(GachaBoxType gachaBoxType)
        {
            if (OpenedBoxCountWithoutGarantee.TryGetValue(gachaBoxType, out var gachaBoxes))
            {
                return gachaBoxes; 
            }

            return 0;
        }
        
        public void IncreaseCountOfGachaBoxOpened(GachaBoxType gachaBoxType, int count)
        {
            if (OpenedBoxCountWithoutGarantee.TryGetValue(gachaBoxType, out var gachaBoxes))
            {
                OpenedBoxCountWithoutGarantee[gachaBoxType] += count;
                return;
            }
            
            OpenedBoxCountWithoutGarantee.Add(gachaBoxType, count);
        }
        
        public void RefreshCountOfGachaBoxOpened(GachaBoxType gachaBoxType)
        {
            if (OpenedBoxCountWithoutGarantee.TryGetValue(gachaBoxType, out var gachaBoxes))
            {
                OpenedBoxCountWithoutGarantee[gachaBoxType] = 0;
            }
        }
    }

 
}