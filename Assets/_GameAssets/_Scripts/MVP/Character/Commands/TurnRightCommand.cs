using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Character.View;

namespace TapAndRun.MVP.Character.Commands
{
    public class TurnRightCommand : ICommand
    {
        private readonly CharacterView _character;

        public TurnRightCommand(CharacterView character)
        {
            _character = character;
        }

        public void Execute()
        {
            _character.TurnAsync(90).Forget();
        }
    }
}