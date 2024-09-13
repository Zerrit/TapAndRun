using System;
using UnityEngine;

namespace TapAndRun.PrallaxBackground
{
    [Serializable]
    public struct BackgroundData
    {
        [field: SerializeField] public Sprite Texture { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
    }
}