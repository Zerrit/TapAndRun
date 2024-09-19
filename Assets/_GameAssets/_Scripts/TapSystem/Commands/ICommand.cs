using Cysharp.Threading.Tasks;

namespace TapAndRun.TapSystem.Commands
{
    public interface ICommand
    {
        UniTask ExecuteAsync();
    }
}