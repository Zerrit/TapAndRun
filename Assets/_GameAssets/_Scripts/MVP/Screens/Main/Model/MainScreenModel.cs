using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Screens.LevelSelect;
using TapAndRun.MVP.Screens.Settings;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Screens.Main.Model
{
    public class MainScreenModel : IMainScreenSelfModel, IMainScreenModel
    {
        public SimpleReactiveProperty<bool> IsDisplaying { get; private set; }

        private readonly ILevelsModel _levelModel;
        private readonly LevelSelectScreen _levelSelectScreen;
        private readonly SettingsPopup _settingsPopup;

        public MainScreenModel(ILevelsModel levelModel, LevelSelectScreen levelSelectScreen, SettingsPopup settingsPopup)
        {
            _levelModel = levelModel;
            _levelSelectScreen = levelSelectScreen;
            _settingsPopup = settingsPopup;
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