using System;
using UnityEngine;

namespace TapAndRun.Configs
{
    [CreateAssetMenu(fileName = "BackgroundConfig", menuName = "Background Config")]
    public class BackgroundsConfig : ScriptableObject
    {
        [field: SerializeField] public BackgroundData[] BackgroundsPresets { get; private set; }
    }

    [Serializable]
    public struct BackgroundData
    {
        [field: SerializeField] public Sprite Texture { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
    }
}