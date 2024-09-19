using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;

namespace TapAndRun.Services.Transition
{
    public interface ITransitionService : IInitializableAsync
    {
        UniTask ShowTransition(CancellationToken token, bool canFinish = false);

        /// <summary>
        /// Закрывает экран перехода, если он в данный момент отображается.
        /// </summary>
        void TryEndTransition();
    }
}