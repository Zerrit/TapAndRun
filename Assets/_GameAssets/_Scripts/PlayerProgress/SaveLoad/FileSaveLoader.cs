using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace TapAndRun.PlayerProgress.SaveLoad
{
    public class FileSaveLoader : ISaveLoader
    {
        private string SavePath => Application.persistentDataPath;
        private string FullPath => Path.Combine(SavePath, $"{FileName}{SaveFileExtension}");
        
        private const string FileName = "Tap&RunPlayerData";
        private const string SaveFileExtension = "json";
        
        public async UniTask SaveAsync(object data)
        {
            await File.WriteAllTextAsync(FullPath, JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));

            Debug.Log("Save data has been written");
        }

        public async UniTask<string> LoadAsync()
        {
            if (!IsFileExist())
            {
                Debug.Log("Save data has not been found");
                return null;
            }
            
            return await File.ReadAllTextAsync(FullPath);
        }
        
        public bool IsFileExist()
        {
            return File.Exists(FullPath);
        }
    }
}