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
        private ICharacterModel _characterModel;
        private ILevelModel _levelModel;
        private IMainScreenModel _mainScreenModel;
        private CharacterPresenter _characterPresenter;
        private LevelsPresenter _levelsPresenter;
        private MainScreenPresenter _mainScreenPresenter;

        [Inject]
        public void Construct(
            ICharacterModel characterModel, 
            ILevelModel levelModel,
            IMainScreenModel mainScreenModel,
            CharacterPresenter characterPresenter,
            LevelsPresenter levelsPresenter,
            MainScreenPresenter mainScreenPresenter)
        {
            _characterModel = characterModel;
            _levelModel = levelModel;
            _mainScreenModel = mainScreenModel;
            _characterPresenter = characterPresenter;
            _levelsPresenter = levelsPresenter;
            _mainScreenPresenter = mainScreenPresenter;
        }

        public void Start()
        {
            _characterModel.Initialize();
            _characterPresenter.Initialize();

            _levelModel.Initialize();
            _levelsPresenter.Initialize();

            _mainScreenModel.Initialize();
            _mainScreenPresenter.Initialize();

            _levelModel.LoadLevel();
        }
    }
}