using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Model
{
    public class LevelsModel : ISelfLevelsModel, ILevelsModel, IDisposable
    {
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

        private CancellationTokenSource _cts;

        private const int NonSelectedLevelId = -1;
        private const int MinDifficulty = 1;

        public UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();
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

        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}