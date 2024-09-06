using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;

namespace TapAndRun.Services.Transition
{
    public interface ITransitionService : IInitializableAsync
    {
        UniTask ShowTransition(CancellationToken token, bool canFinish = false);
        void TryEndTransition();
    }
}