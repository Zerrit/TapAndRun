using System;
using System.Collections.Generic;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.CharacterCamera;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.Views;
using TapAndRun.MVP.TapCommands.Commands;

namespace TapAndRun.MVP.TapCommands
{
    public class TapCommandHandler
    {
        private readonly List<ICommand> _commandsQueue = new ();
        
        private readonly ICharacterModel _character;
        private readonly ICameraModel _cameraModel;
        
        public TapCommandHandler(ICharacterModel character, ICameraModel cameraModel)
        {
            _character = character;
            _cameraModel = cameraModel;
        }

        public void GenerateCommand(IEnumerable<InteractionPoint> interactions)
        {
            _commandsQueue.Clear();
            
            foreach (var interact in interactions)
            {
                switch (interact.CommandType)
                {
                    case InteractType.Jump:
                    {
                        _commandsQueue.Add(new JumpCommand(_character, _cameraModel));
                        break;
                    }
                    case InteractType.TurnLeft:
                    {
                        _commandsQueue.Add(new TurnLeftCommand(_character, _cameraModel));
                        break;
                    }
                    case InteractType.TurnRight:
                    {
                        _commandsQueue.Add(new TurnRightCommand(_character, _cameraModel));
                        break;
                    }

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void ExecuteCommand(int commandIndex)
        {
            _commandsQueue[commandIndex].Execute();
        }
    }
}