using System.Threading;
using Cysharp.Threading.Tasks;

namespace TapAndRun.Services.Transition
{
    public interface ITransitionView
    {
        UniTask ShowAsync(CancellationToken token);
        UniTask HideAsync(CancellationToken token);
    }
}