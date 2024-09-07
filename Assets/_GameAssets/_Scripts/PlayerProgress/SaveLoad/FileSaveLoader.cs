using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace TapAndRun.PlayerProgress.SaveLoad
{
    public class FileSaveLoader : ISaveLoader
    {
#if UNITY_EDITOR
        private string SavePath => Application.dataPath;
#else
        private string SavePath => Application.persistentDataPath;
#endif
        private string FullPath => Path.Combine(SavePath, $"{FileName}{SaveFileExtension}");
        
        private const string FileName = "Tap&RunPlayerData";
        private const string SaveFileExtension = ".json";

        public void Save(string data)
        {
            File.WriteAllText(FullPath, data);

            Debug.Log("Save data has been written instantly");
        }

        public async UniTask SaveAsync(string data, CancellationToken token)
        {
            await File.WriteAllTextAsync(FullPath, data, token);

            Debug.Log("Save data has been written");
        }

        public async UniTask<string> LoadAsync(CancellationToken token)
        {
            if (!IsFileExist())
            {
                Debug.Log("Save data has not been found");
                return null;
            }

            return await File.ReadAllTextAsync(FullPath, token);
        }

        public bool IsFileExist()
        {
            return File.Exists(FullPath);
        }
    }
}