using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.Presenter;
using TapAndRun.MVP.Screens.Main;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TapAndRun.Architecture
{
    public class EntryPoint : MonoBehaviour, IAsyncStartable
    {
        private ILevelsModel _levelModel;
        private IMainScreenModel _mainScreenModel;
        private LevelsPresenter _levelsPresenter;
        private MainScreenPresenter _mainScreenPresenter;

        [Inject]
        public void Construct(
            ILevelsModel levelModel,
            IMainScreenModel mainScreenModel,
            LevelsPresenter levelsPresenter,
            MainScreenPresenter mainScreenPresenter)
        {
            _levelModel = levelModel;
            _mainScreenModel = mainScreenModel;
            _levelsPresenter = levelsPresenter;
            _mainScreenPresenter = mainScreenPresenter;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            _levelModel.Initialize();
            await _levelsPresenter.InitializeAsync(cancellation);

            _mainScreenModel.Initialize();
            _mainScreenPresenter.Initialize();
        }
    }
}