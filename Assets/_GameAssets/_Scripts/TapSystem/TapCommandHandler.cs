using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.CharacterCamera;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.Views;
using TapAndRun.TapSystem.Commands;
using UnityEngine;

namespace TapAndRun.TapSystem
{
    public class TapCommandHandler
    {
        public bool IsBusy { get; private set; }
        
        public int CurrentInteractionIndex { get; private set; }
        public int InteractionCount { get; private set; }
        //TODO Рассмотреть флаг отвачающий за автовыполнение следующей команды, если игрок выполнял тап незадолго до окончания текущего

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
            CurrentInteractionIndex = 0;
            InteractionCount = 0;
            
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

                InteractionCount++;
            }
        }

        public async UniTaskVoid ExecuteCommand()
        {
            if (!CheckAvailability())
            {
                throw new Exception("Попытка запуска команды вопреки доступности!");
            }

            IsBusy = true;
            await _commandsQueue[CurrentInteractionIndex].ExecuteAsync();

            CurrentInteractionIndex++;
            IsBusy = false;
            // TODO Проверка на клик в очереди и автозапуск следующей команды при наличии
        }

        public void ResetCommandIndex()
        {
            CurrentInteractionIndex = 0;
        }

        public bool CheckAvailability()
        {
            if (IsBusy)
            {
                Debug.Log("В данный момент уже выполняется команда");
                return false;
            }

            if(CurrentInteractionIndex >= InteractionCount)
            {
                Debug.Log("Для текущего уровня не осталось команд");
                return false;
            }

            return true;
            
        }
    }
}