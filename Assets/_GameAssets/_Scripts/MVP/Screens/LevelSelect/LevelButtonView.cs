using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.MVP.Screens.LevelSelect
{
    public class LevelButtonView : MonoBehaviour
    {
        [field:SerializeField] public TextMeshProUGUI LevelText { get; private set; }
        [field:SerializeField] public Button Button { get; private set; }
    }
}