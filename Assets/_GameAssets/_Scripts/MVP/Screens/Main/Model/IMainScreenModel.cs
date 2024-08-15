using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Screens.Main.Model
{
    public interface IMainScreenModel
    {
        SimpleReactiveProperty<bool> IsDisplaying { get; }
        
        public void Initialize();
    }
}