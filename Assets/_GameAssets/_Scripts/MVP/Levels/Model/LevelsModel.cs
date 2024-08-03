using System;
using System.Threading;
using Cysharp.Threading.Tasks;

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
        
        public int CurrentInteractionIndex { get; set; }
        public int InteractionCount { get; set; }
        
        public bool IsLevelBuild { private get; set; }

        private CancellationTokenSource _cts;

        public void Initialize()
        {
            LevelCount = 4;
            LastUnlockedLevelId = 0;
            CurrentLevelId = 0;

            _cts = new CancellationTokenSource();
        }

        public void LoadLevel()
        {
            LoadLevelAsync(0, _cts.Token).Forget(); //TODO Заменить уровень на последний доступный
        }

        public async UniTask LoadLevelAsync(int levelId, CancellationToken token)
        {
            CurrentLevelId = levelId;
            OnLevelChanged?.Invoke();

            await UniTask.WaitUntil(() => IsLevelBuild, cancellationToken: token);

            IsLevelBuild = false;
        }

        public void StartLevel()
        {
            OnLevelStarted?.Invoke(); //TODO Привязать к данным
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

            CurrentLevelId++;

            OnLevelCompleted?.Invoke();
        }

        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}