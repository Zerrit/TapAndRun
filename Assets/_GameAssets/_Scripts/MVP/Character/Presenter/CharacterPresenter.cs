using System;
using System.Collections.Generic;
using TapAndRun.CameraLogic;
using TapAndRun.MVP.Character.Commands;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Character.View;
using TapAndRun.MVP.Levels.Model;
using UnityEngine;

namespace TapAndRun.MVP.Character.Presenter
{
    public class CharacterPresenter
    {
        private ICommand _activeCommands;
        
        private readonly List<ICommand> _commandsList = new ();
        
        private readonly ICharacterSelfModel _model;
        private readonly CharacterView _view;
        private readonly CharacterCamera _camera;
        
        public CharacterPresenter(ICharacterSelfModel model, CharacterView view, CharacterCamera camera)
        {
            _model = model;
            _view = view;
            _camera = camera;
        }

        public void Initialize()
        {
            _camera.SetCharacter(_view.CharacterTransform);

            _model.OnReplaced += ReplaceCharacterView;
            _model.IsRunning.OnChanged += SwitchRunStatus;
            _model.OnInteractionsUpdated += UpdateCommands;
            _model.OnInteractionPerformed += ProсcesNextCommand;
        }

        private void ReplaceCharacterView(Vector2 position)
        {
            _view.CharacterTransform.position = position;
        }
        
        private void SwitchRunStatus(bool status)
        {
            if(status) _view.StartMove(_model.MoveDirection, _model.Speed);
        }

        private void UpdateCommands()
        {
            _commandsList.Clear();

            foreach (var interact in _model.LevelInteractions)
            {
                switch (interact)
                {
                    case InteractType.Jump:
                    {
                        _commandsList.Add(new JumpCommand(_view));
                        break;
                    }
                    case InteractType.TurnLeft:
                    {
                        _commandsList.Add(new TurnLeftCommand(_view));
                        break;
                    }
                    case InteractType.TurnRight:
                    {
                        _commandsList.Add(new TurnRightCommand(_view));
                        break;
                    }
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void ProсcesNextCommand(int interactionIndex)
        {
            _activeCommands = _commandsList[interactionIndex]; //TODO Проверка на наличие в очереди
            _activeCommands.Execute();
            Debug.Log("Выполнение команды");
        }
    }
}
