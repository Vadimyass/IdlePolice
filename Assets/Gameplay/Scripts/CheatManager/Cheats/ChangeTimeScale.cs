using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.CheatManager.Cheats
{
    public class ChangeTimeScale : ICheat
    {
        private ICheatManager _cheatManager;

        [Inject]
        private void Construct(ICheatManager cheatManager)
        {
            _cheatManager = cheatManager;
        }

        public void AddCheat()
        {
            _cheatManager.AddCheat<CheatButton>(button =>
            {
                button.SetButtonName("Timescale x2");
                button.SetButtonCallback(() => Time.timeScale = 2);
            });
            
            _cheatManager.AddCheat<CheatButton>(button =>
            {
                button.SetButtonName("Timescale x4");
                button.SetButtonCallback(() => Time.timeScale = 4);
            });
            
            _cheatManager.AddCheat<CheatButton>(button =>
            {
                button.SetButtonName("Timescale x1");
                button.SetButtonCallback(() => Time.timeScale = 1);
            });
            
            _cheatManager.AddCheat<CheatButton>(button =>
            {
                button.SetButtonName("Timescale x0.1");
                button.SetButtonCallback(() => Time.timeScale = 0.1f);
            });
            
            _cheatManager.AddCheat<CheatButton>(button =>
            {
                button.SetButtonName("Timescale x10");
                button.SetButtonCallback(() => Time.timeScale = 10);
            });
            
            _cheatManager.AddCheat<CheatButton>(button =>
            {
                button.SetButtonName("Timescale x100");
                button.SetButtonCallback(() => Time.timeScale = 100);
            });
        }
    }
}