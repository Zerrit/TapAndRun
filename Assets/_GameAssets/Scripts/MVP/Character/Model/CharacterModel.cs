using System.Collections.Generic;
using TapAndRun.Character.Commands;
using TapAndRun.Tools;
using UnityEngine;

namespace TapAndRun.MVP.Character.Model
{
    public class CharacterModel
    {
        public SimpleReactiveProperty<bool> IsRunning { get; set; }
        public SimpleReactiveProperty<bool> IsActive { get; set; }

        public ICommand CurrentCommand { get; private set; }
        public Vector2 MoveDirection = Vector2.up;
        public float Speed { get; set; } = 3f;

        private Queue<ICommand> _commandsQueue = new ();


        public CharacterModel()
        {
            IsActive = new SimpleReactiveProperty<bool>(false);
            IsRunning = new SimpleReactiveProperty<bool>(false);
        }

        public void Initialize()
        {
            IsRunning.Value = true;
        }
        
        /// <summary>
        /// Устанавливает активной следующую команду в очереди, если таковая имеется.
        /// </summary>
        public void SetNextCommand()
        {
            CurrentCommand = _commandsQueue.Dequeue(); //TODO Проверка на наличие в очереди
        }

        public void ProcessCommand()
        {
            CurrentCommand.Execute();
            SetNextCommand();
        }
    }
}
