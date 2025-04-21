using System;
using System.Collections.Generic;
using SolidUtilities.Collections;
using UI;
using UnityEngine;

namespace Gameplay.Configs
{
    [Serializable]
    public class BaseUpgradeConfigInitializator : IPhrase
    {
        public string Key;
        public string Name;
        public double Cost;
        public BuildingName BuildingKey; 
        public SpriteName SpriteName;
        public SerializableDictionary<AdditiveType, double> AdditiveValue;
        public string Description;
        public int DistrictNeed;
    }
    public enum AdditiveType
    {
        Building_income_multiplier,
        Building_car_speed_multiplier,
        Building_interaction_speed,
        Crime_interval,
        Crimes,
        Car_speed,
        Arrest_income,
        Crimes_refresh_rate,
        Police_station_income,
        Interegation_Facility_income,
        Police_station_spead,
        Interegation_Facility_spead,
        All_busses_speed,
        All_income,
    }

    public enum BuildingName
    {
        All,
        Dispatch_center,
        Police_station,
        Police_garage,
        Interegation_facility,
        PretrialDetention_facility,
    }
}