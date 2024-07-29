using System;
using System.Collections.Generic;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.Tools;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Character.Model
{
    public class CharacterModel : ICharacterSelfModel, ICharacterModel
    {
        public event Action<Vector2> OnReplaced; 
        public event Action OnInteractionsUpdated;
        public event Action<int> OnInteractionPerformed;

        public SimpleReactiveProperty<bool> IsRunning { get; private set; }
        public SimpleReactiveProperty<bool> IsInteractive { get; private set; }

        public Vector2 MoveDirection => Vector2.up;
        public float Speed { get; set; } = 3f;

        public IReadOnlyCollection<InteractType> LevelInteractions { get; private set; }
        
        private int _currentInteractionIndex;
        private int _interactionCount;

        public CharacterModel()
        {

        }

        public void Initialize()
        {
            IsInteractive = new SimpleReactiveProperty<bool>(false);
            IsRunning = new SimpleReactiveProperty<bool>(false);
        }

        public void Replace(Vector2 newPosition)
        {
            OnReplaced?.Invoke(newPosition);
        }

        public void PerformInteraction()
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

            Debug.Log("Комманды получены персонажем");
        }
    }
}
