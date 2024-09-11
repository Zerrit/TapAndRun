using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TapAndRun.PlayerData.SaveLoad
{
    public class FileDataStorage : IDataStorage
    {
#if UNITY_EDITOR
        private string SavePath => Application.dataPath;
#else
        private string SavePath => Application.persistentDataPath;
#endif
        private string FullPath => Path.Combine(SavePath, $"{FileName}{SaveFileExtension}");
        
        private const string FileName = "Tap&RunPlayerData";
        private const string SaveFileExtension = ".json";

        public void Write(string data)
        {
            File.WriteAllText(FullPath, data);

            Debug.Log("Save data has been written instantly");
        }

        public async UniTask WriteAsync(string data, CancellationToken token)
        {
            await File.WriteAllTextAsync(FullPath, data, token);

            Debug.Log("Save data has been written");
        }

        public async UniTask<string> ReadAsync(CancellationToken token)
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