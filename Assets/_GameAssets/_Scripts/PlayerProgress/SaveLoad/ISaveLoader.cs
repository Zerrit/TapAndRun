using Cysharp.Threading.Tasks;

namespace TapAndRun.PlayerProgress.SaveLoad
{
    public interface ISaveLoader
    {
        UniTask SaveAsync(object data);
        UniTask<string> LoadAsync();
        bool IsFileExist();
    }
}