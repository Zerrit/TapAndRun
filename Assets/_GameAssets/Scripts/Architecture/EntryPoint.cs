using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Character.Presenter;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.Presenter;
using UnityEngine;
using VContainer;

namespace TapAndRun.Architecture
{
    public class EntryPoint : MonoBehaviour
    {
        private CharacterModel _characterModel;
        private CharacterPresenter _characterPresenter;
        private LevelsModel _levelsModel;
        private LevelsPresenter _levelsPresenter;

        [Inject]
        public void Construct(
            CharacterModel characterModel, 
            CharacterPresenter characterPresenter,
            LevelsModel levelsModel,
            LevelsPresenter levelsPresenter)
        {
            _characterModel = characterModel;
            _characterPresenter = characterPresenter;
            _levelsModel = levelsModel;
            _levelsPresenter = levelsPresenter;
        }

        public void Start()
        {
            _characterPresenter.Initialize();
            _characterModel.Initialize();
        }
    }
}