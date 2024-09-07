using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using TapAndRun.PlayerProgress;
using TapAndRun.PlayerProgress.SaveLoad;
using TapAndRun.PlayerProgress.Serialization;
using UnityEngine;

namespace TapAndRun.Services.Progress
{
    public class DataService : IDataService, IAsyncOnApplicationQuitHandler //TODO
    {
        private readonly Dictionary<string, ISaveLoadable> _saveloadersByKey;

        private readonly IDataSerializer _serializer;
        private readonly ISaveLoader _saveLoader;

        public DataService(IEnumerable<ISaveLoadable> saveLoaders, IDataSerializer serializer, ISaveLoader saveLoader)
        {
            _saveloadersByKey = saveLoaders.ToDictionary(x => x.SaveKey, x => x);
            _serializer = serializer;
            _saveLoader = saveLoader;

            Debug.Log($"Система сохранений обнаружила Сохраняемых сущностей: {_saveloadersByKey.Count}");
        }

        public async UniTask InitializeAsync(CancellationToken token)
        {
            await LoadGameAsync(token);
        }

        public async UniTask OnApplicationQuitAsync()
        {
            await SaveGameAsync();
            Debug.Log("Вызвано сохранение в связи с закрытием приложения!");
        }

        public async UniTask SaveGameAsync()
        {
            var saveFile = new SaveFile();

            foreach (var component in _saveloadersByKey)
            {
                saveFile.Data.Add(component.Value.GetSaveLoadData());
            }

            var serializedSaveFile = await _serializer.SerializeAsync(saveFile);
            await _saveLoader.SaveAsync(serializedSaveFile);

            Debug.Log("Завершено сохранение игры!");
        }

        public async UniTask LoadGameAsync(CancellationToken token)
        {
            if (!_saveLoader.IsFileExist())
            {
                Debug.LogWarning("Не удалось обнаружить файл сохранений");
                return;
            }

            string serializedSaveFile = await _saveLoader.LoadAsync();
            var deserializedSaveFile = await _serializer.DeserializeAsync(serializedSaveFile);

            var saveFile = (SaveFile)deserializedSaveFile; 

            foreach (var data in saveFile.Data)
            {
                if (_saveloadersByKey.ContainsKey(data.Key))
                {
                    _saveloadersByKey[data.Key].RestoreValue(data);
                }
            }

            Debug.Log("Сохранения были успешно загружены!");
        }
    }
}