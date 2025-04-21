namespace UI.Scripts.DialogWindow
{
    public class DialogWindowArguments : UIArguments
    {
        public DialogueName DialogueName => _dialogueName;
        private DialogueName _dialogueName;
        public bool WithSkip { get; private set; }

        public DialogWindowArguments(DialogueName dialogueName,bool withSkip = false)
        {
            WithSkip = withSkip;
            _dialogueName = dialogueName;
        }
    }
}