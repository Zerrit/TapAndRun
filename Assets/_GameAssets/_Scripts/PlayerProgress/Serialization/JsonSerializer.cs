using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace TapAndRun.PlayerProgress.Serialization
{
    public class JsonSerializer : IDataSerializer
    {
        public async UniTask<string> SerializeAsync(object data)
        {
            var text = JsonConvert.SerializeObject(data);

            return await UniTask.FromResult(text);
        }

        public async UniTask<object> DeserializeAsync(string text)
        {
            var data = JsonConvert.DeserializeObject<object>(text);

            return await UniTask.FromResult(data);
        }
    }
}