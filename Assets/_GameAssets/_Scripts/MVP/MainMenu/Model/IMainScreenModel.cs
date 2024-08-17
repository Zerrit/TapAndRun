using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.MainMenu.Model
{
    public interface IMainScreenModel
    {
        SimpleReactiveProperty<bool> IsDisplaying { get; }
        
        public void Initialize();
    }
}