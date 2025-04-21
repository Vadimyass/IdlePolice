using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.CheatManager.Cheats
{
    public class TestCheat : ICheat
    {
        private ICheatManager _cheatManager;

        [Inject]
        public void Construct(ICheatManager cheatManager)
        {
            _cheatManager = cheatManager;
        }

        public void AddCheat()
        {
            _cheatManager.AddCheat<CheatButton>(button =>
            {
                button.SetButtonName("Test cheat!");
                button.SetButtonCallback(
                    () =>
                    {
                        Debug.LogError("test cheat!");
                    }, false);
            });
            
            /*_cheatManager.AddCheat<CheatButton>(button =>
            {
                button.SetButtonName("Open Applovin debugger");
                button.SetButtonCallback(() => MaxSdk.ShowMediationDebugger());
            });*/
        }
    }
}