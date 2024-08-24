using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.CharacterCamera;

namespace TapAndRun.MVP.TapCommands.Commands
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

        public void Execute()
        {
            _character.JumpAsync().Forget();

            if (_cameraModel.Difficulty >= 3)
            {
                _cameraModel.TurnAsync(1).Forget();
            }
        }
    }
}