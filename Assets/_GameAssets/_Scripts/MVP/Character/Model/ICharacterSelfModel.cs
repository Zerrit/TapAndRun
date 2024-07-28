using System;
using System.Collections.Generic;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.Tools;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Character.Model
{
    public interface ICharacterSelfModel
    {
        event Action<Vector2> OnReplaced; 
        event Action OnInteractionsUpdated;
        event Action<int> OnInteractionPerformed;
        
        SimpleReactiveProperty<bool> IsRunning { get; }
        SimpleReactiveProperty<bool> IsInteractive { get; }
        
        Vector2 MoveDirection => Vector2.up;
        float Speed { get; set; }

        IReadOnlyCollection<InteractType> LevelInteractions { get; }
    }
}