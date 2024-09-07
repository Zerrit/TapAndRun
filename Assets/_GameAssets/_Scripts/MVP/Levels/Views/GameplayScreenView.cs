using System;
using TapAndRun.MVP.Levels.Views.Tutorial;
using TapAndRun.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TapAndRun.MVP.Levels.Views
{
    public class GameplayScreenView : ScreenView
    {
        [field: SerializeField] public Button TapButton { get; private set; }
        [field: SerializeField] public GameplayTutorialView TutorialView { get; private set; }
    }
}