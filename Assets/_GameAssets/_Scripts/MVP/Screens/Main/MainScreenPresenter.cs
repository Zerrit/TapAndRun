using System;
using TapAndRun.MVP.Screens.Main.Model;
using TapAndRun.MVP.Screens.Main.Views;
using TapAndRun.MVP.Screens.Settings;

namespace TapAndRun.MVP.Screens.Main
{
    public class MainScreenPresenter: IDisposable
    {
        private readonly IMainScreenSelfModel _model;
        private readonly MainScreenView _view;
        private readonly SettingsPopup _settingsPopup;

        public MainScreenPresenter(IMainScreenSelfModel model, MainScreenView view, SettingsPopup settingsPopup)
        {
            _model = model;
            _view = view;
            _settingsPopup = settingsPopup;
        }

        public void Initialize()
        {
            _view.PlayButton.onClick.AddListener(_model.StartGame);
            _view.SettingsButton.onClick.AddListener(_model.OpenSettings);
            
            _model.IsDisplaying.OnChanged += ChangeDisplaying;
        }

        private void ChangeDisplaying(bool status)
        {
            if (status)
            {
                _view.Show();
            }
            else
            {
                _view.Hide();
            }
        }
        
        public void Dispose()
        {
            _view.PlayButton.onClick.RemoveListener(_model.StartGame);
            _view.SettingsButton.onClick.RemoveListener(_model.OpenSettings);
        }
    }
}