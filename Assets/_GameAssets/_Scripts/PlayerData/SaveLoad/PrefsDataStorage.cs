using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TapAndRun.PlayerData.SaveLoad
{
    public class PrefsDataStorage : IDataStorage
    {
        private const string SettingsFileKey = "Tap&RunPlayerSettings";
        
        public void Write(string data)
        {
            PlayerPrefs.SetString(SettingsFileKey, data);
        }

        public UniTask WriteAsync(string data, CancellationToken token)
        {
            PlayerPrefs.SetString(SettingsFileKey, data);

            return UniTask.CompletedTask;
        }

        public async UniTask<string> Readsync(CancellationToken token)
        {
            var file = PlayerPrefs.GetString(SettingsFileKey);

            return await UniTask.FromResult(file);
        }

        public bool IsFileExist()
        {
            return PlayerPrefs.HasKey(SettingsFileKey);
        }
    }
}