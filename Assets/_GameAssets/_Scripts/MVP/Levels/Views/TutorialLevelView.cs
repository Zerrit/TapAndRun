using UnityEngine;

namespace TapAndRun.MVP.Levels.Views
{
    public class TutorialLevelView : LevelView
    {
        [field:SerializeField] public TutorialInteractPoint TutorialInteractPoint { get; private set; }
    }
}