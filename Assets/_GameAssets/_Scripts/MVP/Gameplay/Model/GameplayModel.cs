using System;
using System.Threading;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Gameplay.Model
{
    public class GameplayModel : IGameplaySelfModel, IGameplayModel, IDisposable
    {
        public event Action OnLevelChanged;
        public event Action OnLevelStarted;

        //public SimpleReactiveProperty<bool> 

        public int CurrentLevelId { get; private set; }
        public int LastUnlockedLevelId { get; private set; }
        public int LevelCount { get; private set; }

        public int CurrentDifficulty { get; private set; }
        public int MaxDifficulty { get; private set; } = 3;

        public int CurrentInteractionIndex { get; set; }
        public int InteractionCount { get; set; }

        private CancellationTokenSource _cts;

        private const int MinDifficulty = 1;

        public void Initialize()
        {
            _cts = new CancellationTokenSource();

            //TODO Load data
            LevelCount = 4;
            LastUnlockedLevelId = 0;
            CurrentLevelId = 0;
            CurrentDifficulty = MinDifficulty;
        }

        public void LoadLevel()
        {
            LoadLevel(0); //TODO Заменить уровень на последний доступный
        }

        public void LoadLevel(int levelId)
        {
            CurrentLevelId = levelId;
            OnLevelChanged?.Invoke();
        }

        public void StartLevel()
        {
            OnLevelStarted?.Invoke(); //TODO Привязать к данным
        }

        public void LoseLevel()
        {
            CurrentInteractionIndex = 0;
            CurrentDifficulty = MinDifficulty;
        }
        
        public void CompleteLevel()
        {
            CurrentInteractionIndex = 0;
            
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