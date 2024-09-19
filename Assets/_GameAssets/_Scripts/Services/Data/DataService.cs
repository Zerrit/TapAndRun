using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TapAndRun.PlayerData;
using TapAndRun.PlayerData.SaveLoad;
using TapAndRun.PlayerData.Serialization;
using UnityEngine;

namespace TapAndRun.Services.Data
{
    public class DataService : IDataService
    {
        private readonly Dictionary<string, IProgressable> _progressablesByKey;
        private readonly Dictionary<string, ISettingable> _settingsUsersByKey;

        private readonly ISerializer _serializer;
        private readonly IDataStorage _dataStorage;
        private readonly IDataStorage _settingsStorage;

        public DataService(IEnumerable<IProgressable> progressables, IEnumerable<ISettingable> settingsUsers)
        {
            _progressablesByKey = progressables.ToDictionary(x => x.SaveKey, x => x);
            _settingsUsersByKey = settingsUsers.ToDictionary(x => x.SaveKey, x => x);

            _serializer = new JsonSerializer();
            _dataStorage = new FileDataStorage();
            _settingsStorage = new PrefsDataStorage();

            Debug.Log($"<color=yellow> Система сохранений обнаружила {_progressablesByKey.Count} сущностей хранящих прогресс </color>");
            Debug.Log($"<color=yellow> Система сохранений обнаружила {_settingsUsersByKey.Count} сущностей использующих настройки </color>");
        }

        public async UniTask InitializeAsync(CancellationToken token)
        {
            await LoadGameAsync(token);
        }

        public async UniTask LoadGameAsync(CancellationToken token)
        {
            await LoadProgressAsync(token);
            await LoadSettingsAsync(token);
        }

        public async UniTask SaveGameAsync(CancellationToken token)
        {
            await SaveProgressAsync(token);
            await SaveSettingsAsync(token);
        }

        public void Save()
        {
            SaveProgress();
            SaveSettings();
        }

        private async UniTask LoadProgressAsync(CancellationToken token)
        {
            if (!_dataStorage.IsFileExist())
            {
                Debug.Log("<color=cyan> Не удалось обнаружить файл сохранений </color>");
                return;
            }

            string serializedSaveFile = await _dataStorage.ReadAsync(token);
            var saveFile = await _serializer.DeserializeAsync<SaveFile>(serializedSaveFile);

            foreach (var data in saveFile.Data)
            {
                if (_progressablesByKey.ContainsKey(data.Key))
                {
                    _progressablesByKey[data.Key].RestoreProgress(data);
                }
            }

            Debug.Log("<color=green> Сохранения были успешно загружены! </color>");
        }

        private async UniTask LoadSettingsAsync(CancellationToken token)
        {
            if (!_settingsStorage.IsFileExist())
            {
                Debug.Log("<color=cyan> Не удалось обнаружить файл настроек </color>");
                return;
            }

            string serializedSettingsFile = await _settingsStorage.ReadAsync(token);
            var settingsFile = await _serializer.DeserializeAsync<SaveFile>(serializedSettingsFile);

            foreach (var data in settingsFile.Data)
            {
                if (_settingsUsersByKey.ContainsKey(data.Key))
                {
                    _settingsUsersByKey[data.Key].RestoreSettings(data);
                }
            }

            Debug.Log("<color=green> Настройки были успешно загружены! </color>");
        }

        private async UniTask SaveProgressAsync(CancellationToken token)
        {
            var progressList = new List<SaveableData>();

            foreach (var component in _progressablesByKey)
            {
                progressList.Add(component.Value.GetProgressData());
            }

            var saveFile = new SaveFile(progressList);
            var serializedSaveFile = await _serializer.SerializeAsync(saveFile);
            await _dataStorage.WriteAsync(serializedSaveFile, token);

            Debug.Log("Завершено сохранение игры!");
        }

        private async UniTask SaveSettingsAsync(CancellationToken token)
        {
            var settingsList = new List<SaveableData>();

            foreach (var component in _settingsUsersByKey)
            {
                settingsList.Add(component.Value.GetSettingsData());
            }

            var settingsFile = new SaveFile(settingsList);
            var serializedSettingsFile = await _serializer.SerializeAsync(settingsFile);
            await _settingsStorage.WriteAsync(serializedSettingsFile, token);

            Debug.Log("Завершено сохранение настроек!");
        }

        private void SaveProgress()
        {
            var progressList = new List<SaveableData>();

            foreach (var component in _progressablesByKey)
            {
                progressList.Add(component.Value.GetProgressData());
            }

            var saveFile = new SaveFile(progressList);
            var serializedSaveFile = _serializer.Serialize(saveFile);
            _dataStorage.Write(serializedSaveFile);

            Debug.Log("Завершено сохранение игры!");
        }

        private void SaveSettings()
        {
            var settingsList = new List<SaveableData>();

            foreach (var component in _settingsUsersByKey)
            {
                settingsList.Add(component.Value.GetSettingsData());
            }

            var settingsFile = new SaveFile(settingsList);
            var serializedSettingsFile = _serializer.Serialize(settingsFile);
            _settingsStorage.Write(serializedSettingsFile);

            Debug.Log("Завершено сохранение настроек!");
        }
    }
}