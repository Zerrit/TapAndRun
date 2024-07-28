using TapAndRun.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.MVP.Screens.Main
{
    public class MainScreenView : ScreenView
    {
        [field:SerializeField] public TextMeshProUGUI Title { get; private set; }
        [field:SerializeField] public Button PlayButton { get; private set; }
        [field:SerializeField] public Button SettingsButton { get; private set; }
    }
}
