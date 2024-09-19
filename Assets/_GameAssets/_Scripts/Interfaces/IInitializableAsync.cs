using System.Threading;
using Cysharp.Threading.Tasks;

namespace TapAndRun.Interfaces
{
    public interface IInitializableAsync
    {
        /// <summary>
        /// Инициализировать объект асинхронно.
        /// </summary>
        UniTask InitializeAsync(CancellationToken token);
    }
}