using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay.Configs;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Newtonsoft.Json;
using SolidUtilities.Collections;
using UI.Scripts.DialogWindow;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace Gameplay.Scripts.DataProfiling
{
    public class DataController
    {
        private static readonly string DATAPATH = (Application.persistentDataPath + "/PlayerData.json");
        private static readonly string RTSDATAPATH = (Application.persistentDataPath + "/RTSData.json");
        private static readonly string TIMEPATH = (Application.persistentDataPath + "/TimeData.json");
        private static readonly string ANALYTICSPATH = (Application.persistentDataPath + "/AnalyticsData.json");
        
        private const string LocalDataParam = "local_data_json";
        private const string LocalTimeParam = "local_time_json";
        private const string LocalAnalyticsParam = "local_analytics_json";
        
        private PlayerPrefsData _playerPrefsData;
        private static ConcurrentQueue<Func<Task>> _saveList = new ConcurrentQueue<Func<Task>>();
        private static bool _isScheduled;
        #region Sheets

        public static void SaveSheetIntoJson(object phrases,string sheetName)
        {
#if UNITY_EDITOR
            var output = JsonConvert.SerializeObject(phrases);
            TextAsset textAsset = new TextAsset(output);
            AssetDatabase.CreateAsset(textAsset,"Assets/Resources/" + sheetName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }
        
        
        public static Dictionary<int, List<T>> ReadSheetFromJsonToDictionary<T>(string sheetName) where T : IPhrase
        {
            var phrases = Resources.Load(sheetName) as TextAsset;
            return JsonConvert.DeserializeObject<Dictionary<int, List<T>>>(phrases.text);
        }
        
        public static Dictionary<DialogueName, List<T>> ReadDialogueSheetFromJsonToDictionary<T>(string sheetName) where T : IPhrase
        {
            var phrases = Resources.Load(sheetName) as TextAsset;
            return JsonConvert.DeserializeObject<Dictionary<DialogueName, List<T>>>(phrases.text);
        }
        
        public static List<T> ReadSheetFromJson<T>(string sheetName)
        {
            var phrases = Resources.Load(sheetName) as TextAsset;
            return JsonConvert.DeserializeObject<List<T>>(phrases.text);
        }
        
        public static Dictionary<int, T> ReadSheetFromJsonToDictionaryWOList<T>(string sheetName) where T: IPhrase
        {
            var phrases = Resources.Load(sheetName) as TextAsset;
            return JsonConvert.DeserializeObject<Dictionary<int, T>>(phrases.text);
        }

        #endregion

        #region UserData

        public static async UniTask<PlayerPrefsData> ReadUserDataFromFileAsync()
        {
#if !BIGD_NEW_SAVE_SYSTEM
            if (!File.Exists(DATAPATH))
            {
                File.WriteAllText(DATAPATH, String.Empty);
                return null;
            }
            
            var userData = await File.ReadAllTextAsync(DATAPATH);
            return JsonConvert.DeserializeObject<PlayerPrefsData>(userData);
#else
            return JsonConvert.DeserializeObject<PlayerPrefsData>(PlayerPrefs.GetString(LocalDataParam));
#endif

        }
        

        public static async UniTask SaveUserDataToFileAsync(PlayerPrefsData playerPrefsData)
        {
            Debug.LogError("save userData");
            await ScheduleSavingAsync(async () =>
            {
#if !BIGD_NEW_SAVE_SYSTEM
                var output = JsonConvert.SerializeObject(playerPrefsData, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                await File.WriteAllTextAsync(DATAPATH, output);
#else
            PlayerPrefs.SetString("LocalDataParam", JsonConvert.SerializeObject(rtsData));
#endif
            });
        }
        

        #endregion

        #region TimeData

        public static async UniTask<Dictionary<int, long>> ReadTimeDataFromFileAsync()
        {
#if !BIGD_NEW_SAVE_SYSTEM
            if (!File.Exists(TIMEPATH))
            {
                File.WriteAllText(TIMEPATH, String.Empty);
                return null;
            }
            
            var userData = await File.ReadAllTextAsync(TIMEPATH);
            
            return JsonConvert.DeserializeObject<Dictionary<int, long>>(userData);
#else
            return JsonConvert.DeserializeObject<Dictionary<int, long>>(PlayerPrefs.GetString(LocalTimeParam));
#endif
        }
        
        public static async UniTask SaveTimeDataToFileAsync(Dictionary<int, long> timeData)
        {
            Debug.LogError("save time");
            await ScheduleSavingAsync(async () =>
            {
#if !BIGD_NEW_SAVE_SYSTEM
                var output = JsonConvert.SerializeObject(timeData, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                await File.WriteAllTextAsync(TIMEPATH, output);
#else
            PlayerPrefs.SetString(LocalTimeParam, JsonConvert.SerializeObject(timeData));
#endif
            });
        }

        #endregion

        /*#region AnalyticsData
        public static async UniTask<AnalyticsInfoModel> ReadAnalyticsDataFromFileAsync()
        {
#if !BIGD_NEW_SAVE_SYSTEM
            if (!File.Exists(ANALYTICSPATH))
            {
                File.WriteAllText(ANALYTICSPATH, String.Empty);
                return null;
            }
            
            var data = await File.ReadAllTextAsync(ANALYTICSPATH);
            
            return JsonConvert.DeserializeObject<AnalyticsInfoModel>(data);
#else
            return JsonConvert.DeserializeObject<AnalyticsInfoModel>(PlayerPrefs.GetString(LocalAnalyticsParam));
#endif
        }

        public static async UniTask SaveAnalyticsDataToFileAsync(AnalyticsInfoModel analyticsInfoModel)
        {
            Debug.LogError("save analytics");
            await ScheduleSavingAsync(async () =>
            {
#if !BIGD_NEW_SAVE_SYSTEM
                var output = JsonConvert.SerializeObject(analyticsInfoModel, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                await File.WriteAllTextAsync(ANALYTICSPATH, output);
#else
            PlayerPrefs.SetString(LocalAnalyticsParam, JsonConvert.SerializeObject(analyticsInfoModel));
#endif
            });
        }

        #endregion*/


        public static async Task ScheduleSavingAsync(Func<Task> saveAction)
        {
            _saveList.Enqueue(saveAction);

            if (!_isScheduled)
            {
                _isScheduled = true;

                while (_saveList.TryDequeue(out var action))
                {
                    try
                    {
                        await action.Invoke();
                        Debug.Log("Item saved");
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error saving item: {ex.Message}");
                        // Handle or log the exception as needed
                    }
                }

                _isScheduled = false;
            }
        }

        public static void ResetProgress()
        {
            File.Delete(DATAPATH);
            File.Delete(TIMEPATH);
            File.Delete(ANALYTICSPATH);
            File.Delete(RTSDATAPATH);
            
            PlayerPrefs.DeleteAll();
        }
    }
}