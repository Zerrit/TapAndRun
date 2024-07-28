using TapAndRun.MVP.Levels.Model;
using TapAndRun.Tools;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Screens.Main
{
    public class MainScreenModel : IMainScreenSelfModel, IMainScreenModel
    {
        public SimpleReactiveProperty<bool> IsDisplaying { get; private set; }

        private readonly ILevelModel _levelModel;
        
        public MainScreenModel(ILevelModel levelModel)
        {
            _levelModel = levelModel;
        }

        public void Initialize()
        {
            IsDisplaying = new SimpleReactiveProperty<bool>(false);
        }
        
        public void StartGame()
        {
            IsDisplaying.Value = false;
            
            _levelModel.StartLevel();
        }

        public void OpenSettings()
        {
            
        }
    }
}