using TapAndRun.MVP.Levels.Views.Tutorial;
using TapAndRun.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.MVP.Levels.Views
{
    public class GameplayScreenView : ScreenView
    {
        [field: SerializeField] public Text LevelText { get; private set; }
        [field: SerializeField] public Text SpeedText { get; private set; }
        [field: SerializeField] public Button TapButton { get; private set; }

        [field: SerializeField] public GameplayTutorialView TutorialView { get; private set; }

        public void UpdateSpeedText(int speedLevel)
        {
            SpeedText.text = speedLevel.ToString();
        }
        
        public void UpdateLevelText(int curentLevelId)
        {
            LevelText.text = $"Level {curentLevelId + 1}";
        }
    }
}