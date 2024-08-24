using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.MainMenu.Model
{
    public interface ISelfMainMenuModel
    {
        public SimpleReactiveProperty<bool> IsDisplaying { get; }

        void StartGame();
    }
}