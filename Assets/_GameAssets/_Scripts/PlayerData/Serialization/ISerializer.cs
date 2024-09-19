using Cysharp.Threading.Tasks;

namespace TapAndRun.PlayerData.Serialization
{
    public interface ISerializer
    {
        string Serialize(object data);

        UniTask<string> SerializeAsync<T>(T data);
        UniTask<T> DeserializeAsync<T>(string data);
    }
}