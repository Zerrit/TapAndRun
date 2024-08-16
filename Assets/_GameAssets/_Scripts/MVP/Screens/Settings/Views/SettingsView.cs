using TapAndRun.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.MVP.Screens.Settings.Views
{
    public class SettingView : PopupView
    {
        [field:SerializeField] public Button CloseButton { get; private set; }
        [field:SerializeField] public CustomToggle AudioToggle { get; private set; }
        [field:SerializeField] public CustomToggle VibroToggle { get; private set; }
        [field:SerializeField] public Button LanguagueButton { get; private set; }
    }
}