using TapAndRun.Tools;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Screens.Main
{
    public interface IMainScreenModel
    {
        SimpleReactiveProperty<bool> IsDisplaying { get; }
        
        public void Initialize();
    }
}