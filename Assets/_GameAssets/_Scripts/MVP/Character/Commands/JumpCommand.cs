using Cysharp.Threading.Tasks;
using TapAndRun.CameraLogic;
using TapAndRun.MVP.Character.View;
using UnityEngine;

namespace TapAndRun.MVP.Character.Commands
{
    public class JumpCommand : ICommand
    {
        private readonly CharacterView _character;
        private readonly CharacterCamera _camera;

        public JumpCommand(CharacterView character, CharacterCamera camera)
        {
            _character = character;
            _camera = camera;
        }

        public void Execute()
        {
            _character.JumpAsync().Forget();

            if (_camera.Difficulty >= 3)
            {
                _camera.TurnAsync(1).Forget();
            }
        }
    }
}