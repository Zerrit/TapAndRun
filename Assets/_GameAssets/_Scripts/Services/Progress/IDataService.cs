using Cysharp.Threading.Tasks;

namespace TapAndRun.Services.Progress
{
    public interface IDataService
    {
        UniTask SaveGame();
        UniTask LoadGame();
    }
}