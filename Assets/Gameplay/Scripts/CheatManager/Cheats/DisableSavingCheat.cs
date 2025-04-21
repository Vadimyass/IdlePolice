using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Zenject;

namespace Gameplay.Scripts.CheatManager.Cheats
{
    public class DisableSavingCheat : ICheat
    {
        private ICheatManager _cheatManager;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;

        [Inject]
        private void Construct(ICheatManager cheatManager,PlayerPrefsSaveManager playerPrefsSaveManager)
        {
            _playerPrefsSaveManager = playerPrefsSaveManager;
            _cheatManager = cheatManager;
        }
        
        public void AddCheat()
        {
            _cheatManager.AddCheat<CheatButton>(button =>
            {
                button.SetButtonName("Disable save");
                button.SetButtonCallback(() =>
                {
                    _playerPrefsSaveManager.DisableSave();
                });
            });
            
            _cheatManager.AddCheat<CheatButton>(button =>
            {
                button.SetButtonName("Delete data");
                button.SetButtonCallback(() =>
                {
                    PlayerPrefsSaveManager.DeleteData();
                });
            });
            
            _cheatManager.AddCheat<CheatButton>(button =>
            {
                button.SetButtonName("Force save");
                button.SetButtonCallback(() =>
                {
                    _playerPrefsSaveManager.ForceSave();
                });
            });
        }
    }
}