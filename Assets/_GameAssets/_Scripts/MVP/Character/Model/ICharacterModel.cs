using System.Collections.Generic;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.Tools;
using TapAndRun.Tools.Reactivity;
using UnityEngine;

namespace TapAndRun.MVP.Character.Model
{
    public interface ICharacterModel
    {
        SimpleReactiveProperty<bool> IsRunning { get; }

        void Initialize();
        void Replace(Vector2 newPosition);
        void SetLevelCommands(IReadOnlyCollection<InteractType> interactions);
        void PerformInteraction();
    }
}