using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.Utils;
using MyBox;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.LockedZoneManagement
{
    public class LockedZoneManager : MonoBehaviour
    {
        [SerializeField] private List<ZonesView> _zonesViews;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void Init()
        {
            foreach (var zonesView in _zonesViews)
            {
                zonesView.Init();
            }
            
            _signalBus.Subscribe<ZoneUnlockSignal>(UpdateButtonViews);
        }

        private void UpdateButtonViews()
        {
            foreach (var zonesView in _zonesViews)
            {
                zonesView.Init();
            }
        }

        public int GetCountOfOpenZones()
        {
            return _zonesViews.Where(x => x.IsOpen == true).ToList().Count+1;
        }

        public bool IsAllZonesOpen()
        {
            return _zonesViews.All(x => x.IsOpen == true);
        }
    }
}