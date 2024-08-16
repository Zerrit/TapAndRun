using TapAndRun.MVP.Screens.Settings.Model;
using TapAndRun.MVP.Screens.Settings.Views;

namespace TapAndRun.MVP.Screens.Settings
{
    public class SettingsPresenter
    {
        private readonly ISelfSettingsModel _model;
        private readonly SettingView _view;

        public SettingsPresenter(ISelfSettingsModel model, SettingView view)
        {
            _model = model;
            _view = view;
        }

        public void Initialize()
        {
            _view.AudioToggle.SetState(_model.AudioStatus);
            _view.VibroToggle.SetState(_model.VibroStatus);

            _model.IsDisplaying.OnChanged += UpdateDisplaying;

            _view.CloseButton.onClick.AddListener(()=> _model.IsDisplaying.Value = false);
            //TODO Подписка на все кнопки окна настроек

            _view.AudioToggle.OnStatusChanged += ChangeAudioStatus;
            _view.VibroToggle.OnStatusChanged += ChangeVibroStatus;
        }

        private void UpdateDisplaying(bool status)
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

        private void ChangeAudioStatus()
        {
            _model.AudioStatus = !_model.AudioStatus;

            _view.AudioToggle.Switch(_model.AudioStatus);
        }

        private void ChangeVibroStatus()
        {
            _model.VibroStatus = !_model.VibroStatus;

            _view.VibroToggle.Switch(_model.VibroStatus);
        }
    }
}