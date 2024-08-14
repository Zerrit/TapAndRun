using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Model
{
    public class LevelsModel : ILevelsSelfModel, ILevelsModel, IDisposable
    {
        public event Action OnLevelChanged;
        public event Action OnLevelStarted;
        public event Action OnLevelCompleted;

        public int CurrentLevelId { get; private set; }
        public int LastUnlockedLevelId { get; private set; }
        public int LevelCount { get; private set; }

        public SimpleReactiveProperty<int> AvailableCrystals { get; private set; }
        public SimpleReactiveProperty<int> CrystalsByLevel { get; private set; }

        public int CurrentDifficulty { get; private set; }
        public int MaxDifficulty { get; private set; } = 3;

        public int CurrentInteractionIndex { get; set; }
        public int InteractionCount { get; set; }

        private CancellationTokenSource _cts;

        private const int MinDifficulty = 1;

        public void Initialize()
        {
            //TODO Load data
            LevelCount = 4;
            LastUnlockedLevelId = 0;
            CurrentLevelId = 0;
            CurrentDifficulty = MinDifficulty;

            AvailableCrystals = new SimpleReactiveProperty<int>(0);
            CrystalsByLevel = new SimpleReactiveProperty<int>(0);

            _cts = new CancellationTokenSource();
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

        public void IncreaseCrystals()
        {
            CrystalsByLevel.Value++;
        }

        public void LoseLevel()
        {
            CurrentInteractionIndex = 0;
            CurrentDifficulty = MinDifficulty;
            CrystalsByLevel.Value = 0;
        }
        
        public void CompleteLevel()
        {
            AvailableCrystals.Value += CrystalsByLevel.Value;
            CrystalsByLevel.Value = 0;
            
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

            OnLevelCompleted?.Invoke();
        }

        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}