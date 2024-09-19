using System;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Views.Tutorial
{
    public class TutorialLevelView : LevelView
    {
        public event Action OnPlayerEnterToTutorial;

        [field:SerializeField] public TutorialInteractPoint[] TutorialInteractPoints { get; private set; }

        public override void Configure(int level)
        {
            base.Configure(level);

            foreach (var point in TutorialInteractPoints)
            {
                point.OnPlayerEntered += () => OnPlayerEnterToTutorial?.Invoke();
            }
        }
    }
}