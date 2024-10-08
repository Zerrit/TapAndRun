﻿using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.CharacterCamera;
using TapAndRun.MVP.CharacterCamera.Model;

namespace TapAndRun.TapSystem.Commands
{
    public class TurnRightCommand : ICommand
    {
        private readonly ICharacterModel _character;
        private readonly ICameraModel _cameraModel;

        public TurnRightCommand(ICharacterModel character, ICameraModel cameraModel)
        {
            _character = character;
            _cameraModel = cameraModel;
        }

        public async UniTask ExecuteAsync()
        {
            _cameraModel.TurnAsync(-1).Forget();

            await _character.TurnAsync(-90);
        }
    }
}