using Gameplay.Scripts.Buildings;
using Gameplay.Scripts.DataProfiling.PrefsData;
using Gameplay.Scripts.LevelManagement;
using Zenject;

namespace Gameplay.Scripts.CheatManager.Cheats
{
    public class AddCurrencyCheat : ICheat
    {
        private ICheatManager _cheatManager;
        private PlayerPrefsSaveManager _playerPrefsSaveManager;
        private LevelController _militaryBaseController;

        [Inject]
        public void Construct(ICheatManager cheatManager,PlayerPrefsSaveManager playerPrefsSaveManager,LevelController militaryBaseController)
        {
            _militaryBaseController = militaryBaseController;
            _playerPrefsSaveManager = playerPrefsSaveManager;
            _cheatManager = cheatManager;
        }
        
        public void AddCheat()
        {
            _cheatManager.AddCheat<CheatButton>((button) =>
            {
                button.SetButtonName("Add 5000 dollars");
                button.SetButtonCallback(() => _playerPrefsSaveManager.PrefsData.CurrenciesModel.ChangeMoney(5000, _militaryBaseController.CurrentLevel.Level),false);
            });
            
            _cheatManager.AddCheat<CheatButton>((button) =>
            {
                button.SetButtonName("Add 50000 dollars");
                button.SetButtonCallback(() => _playerPrefsSaveManager.PrefsData.CurrenciesModel.ChangeMoney(50000, _militaryBaseController.CurrentLevel.Level),false);
            });
            
            _cheatManager.AddCheat<CheatButton>((button) =>
            {
                button.SetButtonName("Add 500000 dollars");
                button.SetButtonCallback(() => _playerPrefsSaveManager.PrefsData.CurrenciesModel.ChangeMoney(500000, _militaryBaseController.CurrentLevel.Level),false);
            });
            
            _cheatManager.AddCheat<CheatButton>((button) =>
            {
                button.SetButtonName("Add 5M dollars");
                button.SetButtonCallback(() => _playerPrefsSaveManager.PrefsData.CurrenciesModel.ChangeMoney(5000000, _militaryBaseController.CurrentLevel.Level),false);
            });
            
            _cheatManager.AddCheat<CheatButton>((button) =>
            {
                button.SetButtonName("Add 500M dollars");
                button.SetButtonCallback(() => _playerPrefsSaveManager.PrefsData.CurrenciesModel.ChangeMoney(500000000, _militaryBaseController.CurrentLevel.Level),false);
            });
            
            _cheatManager.AddCheat<CheatButton>((button) =>
            {
                button.SetButtonName("Add 50 crystals");
                button.SetButtonCallback(() => _playerPrefsSaveManager.PrefsData.CurrenciesModel.IncreaseCrystal(50),false);
            });
            
            _cheatManager.AddCheat<CheatButton>((button) =>
            {
                button.SetButtonName("Add 500 crystals");
                button.SetButtonCallback(() => _playerPrefsSaveManager.PrefsData.CurrenciesModel.IncreaseCrystal(500),false);
            });
        }
    }
}