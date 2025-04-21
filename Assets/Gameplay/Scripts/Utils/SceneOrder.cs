using System;

namespace Gameplay.Scripts.Utils
{
    public static class SceneOrder
    {
        private static string _sceneToOpen;

        static SceneOrder()
        {
            _sceneToOpen = String.Empty;
        }

        public static void OrderScene(string sceneToOpen)
        {
            _sceneToOpen = sceneToOpen;
        }

        public static string GetOrderedScene()
        {
            var orderedScene = _sceneToOpen;
            _sceneToOpen = String.Empty;
            return orderedScene;
        }
    }
}