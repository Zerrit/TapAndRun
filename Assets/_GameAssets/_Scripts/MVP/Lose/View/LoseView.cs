using TapAndRun.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.MVP.Lose.View
{
    public class LoseView : ScreenView
    {
        [field:SerializeField] public Button HomeButton { get; private set; }
        [field:SerializeField] public Button RestartButton { get; private set; }
    }
}