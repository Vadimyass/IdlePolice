
namespace UI.Stack
{
    public class UIStackItem
    {
        public UIScreenController ScreenController { get; }
        public UIArguments UIScreenArgs { get; }
        public UIStack Stack { get; }

        public UIStackItem(UIScreenController screenController, UIArguments uiScreenArgs)
        {
            ScreenController = screenController;
            UIScreenArgs = uiScreenArgs;
            Stack = new UIStack();
        }
    }
}