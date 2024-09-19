using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace TapAndRun.PlayerData.Serialization
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public async UniTask<string> SerializeAsync<T>(T data)
        {
            var text = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return await UniTask.FromResult(text);
        }

        public async UniTask<T> DeserializeAsync<T>(string text)
        {
            var data = JsonConvert.DeserializeObject<T>(text);

            return await UniTask.FromResult(data);
        }
    }
}
