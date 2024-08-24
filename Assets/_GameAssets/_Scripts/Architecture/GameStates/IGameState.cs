using System.Threading;
using Cysharp.Threading.Tasks;

namespace TapAndRun.Architecture.GameStates
{
    public interface IGameState
    {
        UniTask EnterAsync(CancellationToken token);
        UniTask ExitAsync(CancellationToken token);
    }
}