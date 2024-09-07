using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.PlayerProgress;
using TapAndRun.PlayerProgress.SaveLoad;
using TapAndRun.PlayerProgress.Serialization;
using UnityEngine;

namespace TapAndRun.Services.Progress
{
    public class DataService : IDataService //TODO
    {
        private readonly Dictionary<string, ISaveLoadable> _saveloadersByKey;

        private readonly ISerializer _serializer;
        private readonly ISaveLoader _saveLoader;

        public DataService(IEnumerable<ISaveLoadable> saveLoaders, ISerializer serializer, ISaveLoader saveLoader)
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

        public void SaveInstant()
        {
            var progressList = new List<ProgressData>();

            foreach (var component in _saveloadersByKey)
            {
                progressList.Add(component.Value.GetProgressData());
            }

            var saveFile = new SaveFile(progressList);
            var serializedSaveFile = _serializer.Serialize(saveFile);
            _saveLoader.Save(serializedSaveFile);

            Debug.Log("Завершено сохранение игры!");
        }

        public async UniTask SaveGameAsync(CancellationToken token)
        {
            var progressList = new List<ProgressData>();

            foreach (var component in _saveloadersByKey)
            {
                progressList.Add(component.Value.GetProgressData());
            }

            var saveFile = new SaveFile(progressList);
            var serializedSaveFile = await _serializer.SerializeAsync(saveFile);
            await _saveLoader.SaveAsync(serializedSaveFile, token);

            Debug.Log("Завершено сохранение игры!");
        }

        public async UniTask LoadGameAsync(CancellationToken token)
        {
            if (!_saveLoader.IsFileExist())
            {
                Debug.LogWarning("Не удалось обнаружить файл сохранений");
                return;
            }

            string serializedSaveFile = await _saveLoader.LoadAsync(token);
            var saveFile = await _serializer.DeserializeAsync<SaveFile>(serializedSaveFile);

            foreach (var data in saveFile.Data)
            {
                if (_saveloadersByKey.ContainsKey(data.Key))
                {
                    _saveloadersByKey[data.Key].RestoreProgress(data);
                }
            }

            Debug.Log("Сохранения были успешно загружены!");
        }
    }
}