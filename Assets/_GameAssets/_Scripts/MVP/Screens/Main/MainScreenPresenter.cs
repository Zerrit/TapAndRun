using System;

namespace TapAndRun.MVP.Screens.Main
{
    public class MainScreenPresenter: IDisposable
    {
        private readonly IMainScreenSelfModel _model;
        private readonly MainScreenView _view;

        public MainScreenPresenter(IMainScreenSelfModel model, MainScreenView view)
        {
            _model = model;
            _view = view;
        }

        public void Initialize()
        {
            _model.IsDisplaying.OnChanged += ChangeDisplaying;
            _view.PlayButton.onClick.AddListener(_model.StartGame);
            _view.SettingsButton.onClick.AddListener(_model.OpenSettings);
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