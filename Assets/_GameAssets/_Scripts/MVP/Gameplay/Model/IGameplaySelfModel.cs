using System;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Gameplay.Model
{
    public interface IGameplaySelfModel
    {
        event Action OnLevelChanged;
        event Action OnLevelStarted;

        int CurrentLevelId { get; }
        int CurrentInteractionIndex { get; set; }
        int InteractionCount { get; set; }

        int CurrentDifficulty { get;}

        void LoseLevel();
        void CompleteLevel();
    }
}