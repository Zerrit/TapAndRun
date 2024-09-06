using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TapAndRun.PlayerProgress;
using TapAndRun.PlayerProgress.SaveLoad;
using TapAndRun.PlayerProgress.Serialization;
using UnityEngine;

namespace TapAndRun.Services.Progress
{
    public class DataService : IDataService
    {
        private readonly Dictionary<string, ISaveLoadable> _saveloadersByKey;

        private readonly IDataSerializer _serializer;
        private readonly ISaveLoader _saveLoader;

        public DataService(IEnumerable<ISaveLoadable> saveLoaders, IDataSerializer serializer, ISaveLoader saveLoader)
        {
            _serializer = serializer;
            _saveLoader = saveLoader;
            _saveloadersByKey = saveLoaders.ToDictionary(x => x.SaveKey, x => x);
        }

        public async UniTask SaveGame()
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

        public async UniTask LoadGame()
        {
            if (!_saveLoader.IsFileExist())
            {
                Debug.LogError("Не удалось обнаружить файл сохранений");
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