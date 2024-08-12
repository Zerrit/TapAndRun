﻿using Cysharp.Threading.Tasks;
using TapAndRun.CameraLogic;
using TapAndRun.MVP.Character.View;

namespace TapAndRun.MVP.Character.Commands
{
    public class TurnLeftCommand : ICommand
    {
        private readonly CharacterView _character;
        private readonly CharacterCamera _camera;

        public TurnLeftCommand(CharacterView character, CharacterCamera camera)
        {
            _character = character;
            _camera = camera;
        }

        public void Execute()
        {
            _character.TurnAsync(90).Forget();
            _camera.TurnAsync(1).Forget();
        }
    }
}