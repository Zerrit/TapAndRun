using Cysharp.Threading.Tasks;

namespace TapAndRun.PlayerProgress.Serialization
{
    public interface IDataSerializer
    {
        UniTask<string> SerializeAsync(object data);
        UniTask<object> DeserializeAsync(string data);
    }
}