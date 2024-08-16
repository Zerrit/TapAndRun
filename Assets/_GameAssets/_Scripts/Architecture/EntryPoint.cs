using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.Presenter;
using TapAndRun.MVP.Screens.Main;
using TapAndRun.MVP.Screens.Main.Model;
using TapAndRun.MVP.Screens.Settings;
using TapAndRun.MVP.Screens.Settings.Model;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TapAndRun.Architecture
{
    public class EntryPoint : MonoBehaviour, IAsyncStartable
    {
        private ILevelsModel _levelModel;
        private IMainScreenModel _mainScreenModel;
        private ISettingsModel _settingsModel;
        private SettingsPresenter _settingsPresenter;
        private LevelsPresenter _levelsPresenter;
        private MainScreenPresenter _mainScreenPresenter;

        [Inject]
        public void Construct(
            ILevelsModel levelModel,
            IMainScreenModel mainScreenModel,
            ISettingsModel settingsModel,
            SettingsPresenter settingsPresenter,
            LevelsPresenter levelsPresenter,
            MainScreenPresenter mainScreenPresenter)
        {
            _levelModel = levelModel;
            _mainScreenModel = mainScreenModel;
            _settingsModel = settingsModel;
            _settingsPresenter = settingsPresenter;
            _levelsPresenter = levelsPresenter;
            _mainScreenPresenter = mainScreenPresenter;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            _levelModel.Initialize();
            await _levelsPresenter.InitializeAsync(cancellation);

            _settingsModel.Initialize();
            _settingsPresenter.Initialize();
            _mainScreenModel.Initialize();
            _mainScreenPresenter.Initialize();
        }
    }
}