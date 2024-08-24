using TapAndRun.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.MVP.MainMenu.Views
{
    public class MainMenuView : ScreenView
    {
        [field:SerializeField] public TextMeshProUGUI Title { get; private set; }

        [field:SerializeField] public Button SettingsButton { get; private set; }
        [field:SerializeField] public Button PlayButton { get; private set; }
        [field:SerializeField] public Button SkinsShopButton { get; private set; }
        [field:SerializeField] public Button LevelSelectButton { get; private set; }

    }
}
