using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.MainMenu.Model
{
    public interface ISelfMainMenuModel
    {
        public ReactiveProperty<bool> IsDisplaying { get; }

        void StartGame();
    }
}