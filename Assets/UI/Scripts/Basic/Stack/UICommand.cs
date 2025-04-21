namespace UI.Stack
{
    public class UICommand
    {
        public enum Type
        {
            Undefined,
            Show,
            Hide,
            HideAll
        }

        public readonly Type CommandType;
        public readonly UIScreenController UIScreenController;
        public readonly UIArguments UIScreenArgs;
        
        public UICommand(Type commandType, UIScreenController uiScreenController, UIArguments uiScreenArgs = null)
        {
            CommandType = commandType;
            UIScreenController = uiScreenController;
            UIScreenArgs = uiScreenArgs;
        }
    }
}