using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Screens.Main.Model
{
    public interface IMainScreenSelfModel
    {
        public SimpleReactiveProperty<bool> IsDisplaying { get; }

        void StartGame();
        void OpenSettings();
    }
}