using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.PlayerData;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Model
{
    public class LevelsModel : ISelfLevelsModel, ILevelsModel, IProgressable, ISettingsUser
    {
        public string SaveKey => "Levels";

        public event Action OnLevelChanged;
        public event Action OnLevelFailed;

        public BoolReactiveProperty IsDisplaying { get; private set; } //TODO TEST

        public TriggerReactiveProperty StartupTrigger { get; private set; }
        public TriggerReactiveProperty ResetLevelTrigger { get; private set; } // TODO TEST

        public int CurrentLevelId
        {
            get
            {
                if (_currentLevel == NonSelectedLevelId)
                {
                    _currentLevel = LastUnlockedLevelId;
                }

                return _currentLevel;
            }
            set => _currentLevel = Mathf.Clamp(value, 0, LevelCount - 1);
        }

        public int LastUnlockedLevelId { get; private set; }
        public int LevelCount { get; set; }

        public bool IsTutorialComplete { get; set; }

        public int CurrentDifficulty { get; private set; }
        public int MaxDifficulty { get; } = 3; //TODO Вынести в конфиг

        private int _currentLevel;
        
        private const int NonSelectedLevelId = -1;
        private const int MinDifficulty = 1;

        public UniTask InitializeAsync(CancellationToken token)
        {
            IsDisplaying = new BoolReactiveProperty();

            StartupTrigger = new TriggerReactiveProperty();
            ResetLevelTrigger = new TriggerReactiveProperty();

            _currentLevel = NonSelectedLevelId;
            CurrentDifficulty = MinDifficulty;
            
            return UniTask.CompletedTask;
        }

        SaveableData IProgressable.GetProgressData()
        {
            return new (SaveKey, new object[] {LastUnlockedLevelId});
        }

        void IProgressable.RestoreProgress(SaveableData loadData)
        {
            if (loadData?.Data == null || loadData.Data.Length < 1)
            {
                Debug.LogError($"Can't restore Wallet data");
                return;
            }

            LastUnlockedLevelId = Convert.ToInt32(loadData.Data[0]);
        }
        
        SaveableData ISettingsUser.GetSettingsData()
        {
            return new(SaveKey, new object[] {IsTutorialComplete});
        }

        void ISettingsUser.RestoreSettings(SaveableData data)
        {
            if (data?.Data == null || data.Data.Length < 1)
            {
                Debug.LogError($"Can't restore LevelsModel settings");
                return;
            }

            IsTutorialComplete = Convert.ToBoolean(data.Data[0]);
        }

        public void SelectLevel(int levelId) //TODO Проверка доступности уровня (изменить тип на bool)
        {
            if (levelId == CurrentLevelId)
            {
                ResetLevelTrigger.Trigger();
            }
            else
            {
                CurrentLevelId = levelId;
                OnLevelChanged?.Invoke();
            }
        }

        public void LoseLevel()
        {
            CurrentDifficulty = MinDifficulty;

            OnLevelFailed?.Invoke();
        }

        public void CompleteLevel()
        {
            if (LevelCount - 1 <= CurrentLevelId)
            {
                throw new Exception("Уровни закончились");
            }

            if (LastUnlockedLevelId == CurrentLevelId)
            {
                LastUnlockedLevelId++;
            }

            CurrentDifficulty = Mathf.Clamp(++CurrentDifficulty, MinDifficulty, MaxDifficulty);
            CurrentLevelId++;
        }

        public bool CheckLevelExist(int levelId)
        {
            return (levelId < LevelCount);
        }


    }
}