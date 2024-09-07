using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;

namespace TapAndRun.Services.Progress
{
    public interface IDataService : IInitializableAsync
    {
        void SaveInstant();
        UniTask SaveGameAsync(CancellationToken token);
        UniTask LoadGameAsync(CancellationToken token);
    }
}