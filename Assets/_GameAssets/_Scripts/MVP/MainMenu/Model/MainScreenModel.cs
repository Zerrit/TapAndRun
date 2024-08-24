using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.MainMenu.Model
{
    public class MainScreenModel : IMainScreenSelfModel, IMainScreenModel
    {
        public SimpleReactiveProperty<bool> IsDisplaying { get; private set; }

        public MainScreenModel()
        {

        }

        public void Initialize()
        {
            IsDisplaying = new SimpleReactiveProperty<bool>(false);
        }
    }
}