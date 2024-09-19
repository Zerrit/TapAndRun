using System.Threading;
using Cysharp.Threading.Tasks;

namespace TapAndRun.PlayerData.SaveLoad
{
    public interface IDataStorage
    {
        bool IsFileExist();
        void Write(string data);

        UniTask WriteAsync(string data, CancellationToken token);
        UniTask<string> ReadAsync(CancellationToken token);
    }
}