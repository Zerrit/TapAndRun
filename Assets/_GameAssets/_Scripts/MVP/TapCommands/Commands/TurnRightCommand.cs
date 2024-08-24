using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.CharacterCamera;

namespace TapAndRun.MVP.TapCommands.Commands
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

        public void Execute()
        {
            _character.TurnAsync(-90).Forget();
            _cameraModel.TurnAsync(-1).Forget();
        }
    }
}