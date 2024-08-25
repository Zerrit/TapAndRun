using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Model
{
    public class LevelsModel : ISelfLevelsModel, ILevelsModel, IDisposable
    {
        public event Action OnLevelReseted;
        public event Action OnLevelChanged;
        public event Action OnLevelStarted;
        public event Action OnLevelFailed;

        public int CurrentLevelId { get; private set; }
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

        public void StartGameplay()
        {
            OnLevelStarted?.Invoke();
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
                return;
            }

            if (LastUnlockedLevelId == CurrentLevelId)
            {
                LastUnlockedLevelId++;
            }

            CurrentDifficulty = Mathf.Clamp(++CurrentDifficulty, MinDifficulty, MaxDifficulty);
            CurrentLevelId++;
        }

        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}