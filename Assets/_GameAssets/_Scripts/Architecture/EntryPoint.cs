using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Character.Presenter;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.Presenter;
using TapAndRun.MVP.Screens.Main;
using UnityEngine;
using VContainer;

namespace TapAndRun.Architecture
{
    public class EntryPoint : MonoBehaviour
    {
        private ILevelModel _levelModel;
        private IMainScreenModel _mainScreenModel;
        private LevelsPresenter _levelsPresenter;
        private MainScreenPresenter _mainScreenPresenter;

        [Inject]
        public void Construct(
            ILevelModel levelModel,
            IMainScreenModel mainScreenModel,
            LevelsPresenter levelsPresenter,
            MainScreenPresenter mainScreenPresenter)
        {
            _levelModel = levelModel;
            _mainScreenModel = mainScreenModel;
            _levelsPresenter = levelsPresenter;
            _mainScreenPresenter = mainScreenPresenter;
        }

        public void Start()
        {
            _levelModel.Initialize();
            _levelsPresenter.Initialize();

            _mainScreenModel.Initialize();
            _mainScreenPresenter.Initialize();

            _levelModel.LoadLevel();
        }
    }
}