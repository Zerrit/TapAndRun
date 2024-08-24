using System.Threading;
using Cysharp.Threading.Tasks;

namespace TapAndRun.Interfaces
{
    public interface IInitializableAsync
    {
        UniTask InitializeAsync(CancellationToken token);
    }
}