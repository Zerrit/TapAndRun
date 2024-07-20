using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Character.View;
using UnityEngine;
using VContainer.Unity;

namespace TapAndRun.MVP.Character.Presenter
{
    public class CharacterPresenter
    {
        private readonly CharacterModel _model;
        private readonly CharacterView _view;
        
        public CharacterPresenter(CharacterModel model, CharacterView view)
        {
            _model = model;
            _view = view;
        }

        public void Initialize()
        {
            _model.IsRunning.OnChanged += SwitchRunStatus;
        }

        private void SwitchRunStatus(bool status)
        {
            if(status) _view.StartMove(_model.MoveDirection, _model.Speed);
        }
    }
}
