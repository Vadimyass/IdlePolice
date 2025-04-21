using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Gameplay.Scripts.DataProfiling;
using UI.Scripts.ShopWindow.Gacha;

namespace Gameplay.Configs
{
    public class EconomyConfig : IConfig
    {
        private List<DefaultConfigInitializer> _items = new ();
        
        public void LoadConfig(string sheetName)
        {
            _items = DataController.ReadSheetFromJson<DefaultConfigInitializer>(sheetName);
        }

        public float GetItemByKey(EconomyEnum key)
        {
            var valueInitializer = _items.FirstOrDefault(x => x.Key == key.ToString());
            if (valueInitializer != null && float.TryParse(valueInitializer.Pair[0],NumberStyles.Any,CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            return -999;
        }
        
        public float GetItemByKey(EconomyEnum key, int level)
        {
            var valueInitializer = _items.FirstOrDefault(x => x.Key == key.ToString());
            if (valueInitializer != null && float.TryParse(valueInitializer.Pair[level],NumberStyles.Any,CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            return -999;
        }

        public float GetDistrictPriceByIndex(int index, int baseID)
        {
            switch (index)
            {
                case 1:
                    return GetItemByKey(EconomyEnum.District_unlock_cost_2,baseID);
                case 2:
                    return GetItemByKey(EconomyEnum.District_unlock_cost_3,baseID);
                case 3:
                    return GetItemByKey(EconomyEnum.District_unlock_cost_4,baseID);
                case 4:
                    return GetItemByKey(EconomyEnum.District_unlock_cost_5,baseID);
                case 5:
                    return GetItemByKey(EconomyEnum.District_unlock_cost_6,baseID);
                case 6:
                    return GetItemByKey(EconomyEnum.District_unlock_cost_7,baseID);
                case 7:
                    return GetItemByKey(EconomyEnum.District_unlock_cost_8,baseID);
                case 8:
                    return GetItemByKey(EconomyEnum.District_unlock_cost_9,baseID);
            }

            return -999;
        }
    }


    public enum EconomyEnum
    {
        Bus_spead,
        Bus_capacity_start,
        District_unlock_cost_1,
        District_unlock_cost_2,
        District_unlock_cost_3,
        District_unlock_cost_4,
        District_unlock_cost_5,
        District_unlock_cost_6,
        District_unlock_cost_7,
        District_unlock_cost_8,
        District_unlock_cost_9,
        Start_Crimes_refresh_spead,
        New_LVL_unlock_cost,
    }
}