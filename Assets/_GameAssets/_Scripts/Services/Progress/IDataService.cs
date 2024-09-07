using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;

namespace TapAndRun.Services.Progress
{
    public interface IDataService : IInitializableAsync
    {
        UniTask SaveGameAsync();
        UniTask LoadGameAsync(CancellationToken token);
    }
}