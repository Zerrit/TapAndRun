﻿using UnityEngine;

namespace TapAndRun.PrallaxBackground.MatrixBackground
{
    public class BackgroundSymbol : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer Texture { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
    }
}