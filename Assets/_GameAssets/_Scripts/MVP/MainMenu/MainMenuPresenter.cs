using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.MVP.MainMenu.Model;
using TapAndRun.MVP.MainMenu.Views;
using TapAndRun.MVP.Settings.Model;

namespace TapAndRun.MVP.MainMenu
{
    public class MainMenuPresenter: IInitializableAsync, IDisposable
    {
        private CancellationTokenSource _cts;

        private readonly ISelfMainMenuModel _model;
        private readonly ISettingsModel _settingsModel;
        private readonly MainMenuView _view;

        public MainMenuPresenter(ISelfMainMenuModel model, ISettingsModel settingsModel, MainMenuView view)
        {
            _model = model;
            _view = view;
            _settingsModel = settingsModel;
        } 

        public UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();

            _model.IsDisplaying.OnChanged += UpdateDisplaying;
            _view.PlayButton.onClick.AddListener(_model.StartGame);
            _view.SettingsButton.onClick.AddListener(OpenSettings);

            return UniTask.CompletedTask;
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

        private void OpenSettings()
        {
            _settingsModel.IsDisplaying.Value = true;
        }

        public void Dispose()
        {
            _model.IsDisplaying.OnChanged -= UpdateDisplaying;
            _view.PlayButton.onClick.RemoveListener(_model.StartGame);
            _view.SettingsButton.onClick.RemoveListener(OpenSettings);
        }
    }
}