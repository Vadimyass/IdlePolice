using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Scripts.Utils
{
    public static class SceneManagement
    {
        public static void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public static void RestartCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        public static SceneName GetCurrentSceneName()
        {
            return Enum.Parse<SceneName>(SceneManager.GetActiveScene().name);
        }

        public static void LoadBootScene()
        {
            SceneManager.LoadScene(0);
        }

        public enum SceneName
        {
            Default,
            BootScene,
            GameplayScene,
        }

        // public async UniTask LoadAsyncScene(string sceneName,Action<float> progress)
        // {
        //     var sceneOperationLoad = SceneManager.LoadSceneAsync(sceneName);
        //     sceneOperationLoad.allowSceneActivation = false;
        //     var isLoadedGame = false;
        //
        //     _signalBus.Subscribe<GameLoadedSignal>(SetLoadedGame);
        //
        //     void SetLoadedGame()
        //     {
        //         Debug.LogError("loaded game");
        //         isLoadedGame = true;
        //     }
        //     while (sceneOperationLoad.isDone == false)
        //     {
        //         if (sceneOperationLoad.progress >= 0.9f)
        //         {
        //             break;
        //         }
        //         Debug.LogError(sceneOperationLoad.progress + "    " + sceneOperationLoad.isDone);
        //         progress?.Invoke(sceneOperationLoad.progress / 0.66f);
        //         await UniTask.Yield();
        //     }
        //     
        //     progress?.Invoke(0.73f);
        //     Debug.LogError("loaded scene");
        //     await UniTask.WaitWhile(() => isLoadedGame == false);
        //     Debug.LogError("loaded scene succesfully");
        //     sceneOperationLoad.allowSceneActivation = true;
        //     progress?.Invoke(1);
        //
        //     _signalBus.TryUnsubscribe<GameLoadedSignal>(SetLoadedGame);
        //     await UniTask.CompletedTask;
        // }

        public static void RestartCurrentSceneAsync()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }
}