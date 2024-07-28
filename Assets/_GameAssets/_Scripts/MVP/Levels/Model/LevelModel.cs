using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Character.Model;
using TapAndRun.Tools;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Model
{
    public class LevelModel : ILevelsSelfModel, ILevelModel, IDisposable
    {
        public event Action OnLevelChanged;
        public event Action OnLevelCompleted;
        public event Action OnPlayerClicked;
        
        public SimpleReactiveProperty<bool> IsScreenDisplaying { get; private set; }
        
        public int CurrentLevelId { get; private set; }

        public int LastUnlockedLevelId { get; private set; }
        public int LevelCount { get; private set; }
        public bool IsLevelBuild { private get; set; }

        private CancellationTokenSource _cts;
        
        private readonly ICharacterModel _characterModel;

        public LevelModel(ICharacterModel characterModel)
        {
            _characterModel = characterModel;
        }

        public void Initialize()
        {
            LevelCount = 2;
            LastUnlockedLevelId = 0;
            CurrentLevelId = 0;

            IsScreenDisplaying = new SimpleReactiveProperty<bool>(false);

            _cts = new CancellationTokenSource();
        }

        public void LoadLevel()
        {
            ChangeLevelAsync(0, _cts.Token).Forget(); //TODO Заменить уровень на последний доступный
        }
        
        public async UniTask ChangeLevelAsync(int levelId, CancellationToken token)
        {
            CurrentLevelId = levelId;
            OnLevelChanged?.Invoke();

            await UniTask.WaitUntil(() => IsLevelBuild, cancellationToken: token);

            IsLevelBuild = false;
        }

        public void StartLevel()
        {
            IsScreenDisplaying.Value = true;

            _characterModel.IsRunning.Value = true;
        }

        public void CompleteLevel()
        {
            if (LevelCount - 1 > CurrentLevelId)
            {
                if (LastUnlockedLevelId == CurrentLevelId)
                {
                    LastUnlockedLevelId++;
                }

                CurrentLevelId++;
                OnLevelCompleted?.Invoke();
            }
        }
        
        public void SetCommands(List<InteractType> interactions)
        {
            _characterModel.SetLevelCommands(interactions);
        }

        public void ApplyClick()
        {
            Debug.Log("Тап по экрану");
            
            _characterModel.PerformInteraction();
            OnPlayerClicked?.Invoke();
        }

        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}