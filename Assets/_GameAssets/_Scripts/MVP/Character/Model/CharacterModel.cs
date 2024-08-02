using System;
using System.Collections.Generic;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.Tools;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Character.Model
{
    public class CharacterModel
    {
        public event Action<Vector2> OnReplaced; 
        public event Action<int> OnInteractionPerformed;
        public event Action OnInteractionsUpdated;

        public SimpleReactiveProperty<bool> IsRunning { get; private set; }

        public Vector2 MoveDirection => Vector2.up;
        public float Speed { get; set; } = 3f;

        public IReadOnlyCollection<InteractType> LevelInteractions { get; private set; }
        
        private int _currentInteractionIndex;
        private int _interactionCount;

        public void Initialize()
        {
            IsRunning = new SimpleReactiveProperty<bool>(false);
        }

        public void MoveTo(Vector2 newPosition)
        {
            OnReplaced?.Invoke(newPosition);
        }

        public void PerformClick()
        {
            if (_currentInteractionIndex >= _interactionCount)
            {
                return;
            }

            OnInteractionPerformed?.Invoke(_currentInteractionIndex);
            _currentInteractionIndex++;
        }

        public void SetLevelCommands(IReadOnlyCollection<InteractType> interactions)
        {
            LevelInteractions = interactions;
            _interactionCount = LevelInteractions.Count;
            _currentInteractionIndex = 0;

            OnInteractionsUpdated?.Invoke();
        }
    }
}
