using UnityEngine;

namespace TapAndRun.MVP.Levels.Views.Tutorial
{
    public class TutorialLevelView : LevelView
    {
        [field:SerializeField] public TutorialInteractPoint TutorialInteractPoint { get; private set; }
    }
}