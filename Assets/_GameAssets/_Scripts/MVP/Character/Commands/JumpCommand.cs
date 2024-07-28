using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Character.View;

namespace TapAndRun.MVP.Character.Commands
{
    public class JumpCommand : ICommand
    {
        private readonly CharacterView _character;

        public JumpCommand(CharacterView character)
        {
            _character = character;
        }

        public void Execute()
        {
            _character.JumpAsync().Forget();
        }
    }
}