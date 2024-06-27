using System.Collections.Generic;
using TapAndRun.Character.Commands;
using UnityEngine;

namespace TapAndRun.Character.Model
{
    public class CharacterModel
    {
        public bool IsActive { get; set; }

        public ICommand CurrentCommand { get; private set; }

        private Queue<ICommand> _commandsQueue = new ();
        private Vector2 _moveDirection = Vector2.up;
        private float _speed;

        public CharacterModel()
        {
            
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
