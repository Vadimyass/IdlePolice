namespace Gameplay.Scripts.CheatManager
{
    public class CheatButtonPopup : CheatButton
    {
        private CheatPopupBase _cheatPopup;
        
        public CheatButtonPopup SetPopup(CheatPopupBase cheatPopupBase)
        {
            _cheatPopup = cheatPopupBase;
            _cheatPopup.gameObject.SetActive(false);
            _button.onClick.AddListener(OpenPopup);
            return this;
        }

        private void OpenPopup()
        {
            if (_cheatPopup == null) return;
            _cheatPopup.gameObject.SetActive(true);
        }

        public override void Dispose()
        {
            if (_cheatPopup != null)
            {
                Destroy(_cheatPopup.gameObject);
            }
            
            base.Dispose();
            
        }
    }
}