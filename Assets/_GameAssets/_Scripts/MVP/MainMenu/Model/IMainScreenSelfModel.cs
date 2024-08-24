using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.MainMenu.Model
{
    public interface IMainScreenSelfModel
    {
        public SimpleReactiveProperty<bool> IsDisplaying { get; }
    }
}