using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;

namespace TapAndRun.Services.Data
{
    public interface IDataService : IInitializableAsync
    {
        void Save();

        UniTask SaveGameAsync(CancellationToken token);
        UniTask LoadGameAsync(CancellationToken token);
    }
}