using TapAndRun.PrallaxBackground;
using UnityEngine;

namespace TapAndRun.Configs
{
    [CreateAssetMenu(fileName = "BackgroundConfig", menuName = "Parallax View/Background Config")]
    public class BackgroundsConfig : ScriptableObject
    {
        [field: SerializeField] public BackgroundData[] BackgroundsPresets { get; private set; }
    }
}