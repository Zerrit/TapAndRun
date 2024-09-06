using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.PlayerProgress;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Model
{
    public class LevelsModel : ISelfLevelsModel, ILevelsModel, ISaveLoadable
    {
        public string SaveKey => "Levels";

        public event Action OnLevelReseted;
        public event Action OnLevelChanged;
        public event Action OnLevelFailed;

        public TriggerReactiveProperty StartupTrigger { get; private set; }
        public TriggerReactiveProperty RemoveTrigger { get; private set; }

        public int CurrentLevelId { get; set; }
        public int LastUnlockedLevelId { get; private set; }
        public int LevelCount { get; set; }

        public int CurrentDifficulty { get; private set; }
        public int MaxDifficulty { get; } = 3; //TODO Вынести в конфиг

        private const int NonSelectedLevelId = -1;
        private const int MinDifficulty = 1;

        public UniTask InitializeAsync(CancellationToken token)
        {
            StartupTrigger = new TriggerReactiveProperty();
            RemoveTrigger = new TriggerReactiveProperty();

            //TODO Load data
            LastUnlockedLevelId = 0;
            CurrentLevelId = NonSelectedLevelId;
            CurrentDifficulty = MinDifficulty;
            
            return UniTask.CompletedTask;
        }

        public void PrepeareCurrentLevel()
        {
            if (CurrentLevelId == NonSelectedLevelId)
            {
                PrepareLevel(LastUnlockedLevelId);
            }
            else
            {
                OnLevelReseted?.Invoke();
            }
        }

        public void PrepareLevel(int levelId)
        {
            //TODO Проверка доступности уровня
            
            if (levelId == CurrentLevelId)
            {
                OnLevelReseted?.Invoke();
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

        SaveLoadData ISaveLoadable.GetSaveLoadData()
        {
            return new SaveLoadData(SaveKey, new object[] {LastUnlockedLevelId});
        }

        void ISaveLoadable.RestoreValue(SaveLoadData loadData)
        {
            if (loadData?.Data == null || loadData.Data.Length < 1)
            {
                Debug.LogError($"Can't restore Wallet data");
                return;
            }

            LastUnlockedLevelId = (int)loadData.Data[0];
        }
    }
}