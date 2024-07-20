using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.UI
{
    public class MainScreen : ScreenView
    {
        [field:SerializeField] public TextMeshProUGUI Title { get; private set; }
        [field:SerializeField] public Button PlayButton { get; private set; }
        [field:SerializeField] public Button SettingsButton { get; private set; }
    }
}
