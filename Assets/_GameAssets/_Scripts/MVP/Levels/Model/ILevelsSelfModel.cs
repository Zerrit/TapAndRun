using System;
using System.Collections.Generic;
using TapAndRun.Tools;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Levels.Model
{
    public interface ILevelsSelfModel
    {
        event Action OnLevelChanged;
        event Action OnLevelStarted;
        event Action OnLevelCompleted;

        int CurrentLevelId { get; }
        int CurrentInteractionIndex { get; set; }
        int InteractionCount { get; set; }

        int CurrentDifficulty { get;}

        void IncreaseCrystals();
        void LoseLevel();
        void CompleteLevel();
    }
}