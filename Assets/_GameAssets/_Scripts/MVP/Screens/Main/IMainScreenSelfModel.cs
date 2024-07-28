using TapAndRun.Tools;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Screens.Main
{
    public interface IMainScreenSelfModel
    {
        public SimpleReactiveProperty<bool> IsDisplaying { get; }

        void StartGame();
        void OpenSettings();
    }
}