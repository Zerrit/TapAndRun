using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Screens.Main.Model;
using TapAndRun.MVP.Screens.Main.Views;
using TapAndRun.MVP.Screens.Settings;
using TapAndRun.MVP.Screens.Settings.Model;

namespace TapAndRun.MVP.Screens.Main
{
    public class MainScreenPresenter: IDisposable
    {
        private CancellationTokenSource _cts;
        
        private readonly IMainScreenSelfModel _model;
        private readonly MainScreenView _view;
        private readonly ISettingsModel _settingsModel;
        private readonly ILevelsModel _levelModel;

        public MainScreenPresenter(IMainScreenSelfModel model, MainScreenView view, 
            ISettingsModel settingsModel, ILevelsModel levelModel)
        {
            _model = model;
            _view = view;
            _settingsModel = settingsModel;
            _levelModel = levelModel;
        } 

        public void Initialize()
        {
            _cts = new CancellationTokenSource();
            
            _view.PlayButton.onClick.AddListener(StartLevel);
            _view.SettingsButton.onClick.AddListener(OpenSettings);
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

        private void StartLevel()
        {
            StartLevelAsync(_cts.Token).Forget();
            
            async UniTaskVoid StartLevelAsync(CancellationToken token)
            {
                await _view.HideAsync(token);

                _levelModel.StartLevel();
            }
        }

        private void OpenSettings()
        {
            _settingsModel.IsDisplaying.Value = true;
        }

        private void CloseSettings()
        {
            
        }
        
        public void Dispose()
        {
            _view.PlayButton.onClick.RemoveListener(StartLevel);
            _view.SettingsButton.onClick.RemoveListener(OpenSettings);
        }
    }
}