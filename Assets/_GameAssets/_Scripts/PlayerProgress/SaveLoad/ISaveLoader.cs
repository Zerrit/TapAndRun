using System.Threading;
using Cysharp.Threading.Tasks;

namespace TapAndRun.PlayerProgress.SaveLoad
{
    public interface ISaveLoader
    {
        void Save(string data);
        UniTask SaveAsync(string data, CancellationToken token);
        UniTask<string> LoadAsync(CancellationToken token);
        bool IsFileExist();
    }
}