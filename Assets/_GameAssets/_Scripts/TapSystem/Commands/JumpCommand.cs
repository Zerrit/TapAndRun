using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.CharacterCamera.Model;
using UnityEngine;

namespace TapAndRun.TapSystem.Commands
{
    public class JumpCommand : ICommand
    {
        private readonly ICharacterModel _character;
        private readonly ICameraModel _cameraModel;

        public JumpCommand(ICharacterModel character, ICameraModel cameraModel)
        {
            _character = character;
            _cameraModel = cameraModel;
        }

        public async UniTask ExecuteAsync()
        {
            if (_cameraModel.CurrentMode == CameraMode.Random)
            {
                _cameraModel.TurnAsync(1).Forget();
            }

            await _character.JumpAsync();
        }
    }
}