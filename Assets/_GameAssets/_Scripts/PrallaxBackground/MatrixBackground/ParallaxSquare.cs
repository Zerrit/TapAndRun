﻿using UnityEngine;

namespace TapAndRun.PrallaxBackground.MatrixBackground
{
    public class ParallaxSquare : MonoBehaviour
    {
        [SerializeField] private BackgroundSymbol[] _symbols;

        private static readonly int Speed = Animator.StringToHash("Speed");

        public void Initialize(float squareSize, float symbolsSize)
        {
            foreach (var symbol in _symbols)
            {
                var randomAngle = Random.Range(0f, 360f);
                var randomXPos = Random.Range(-squareSize / 2f, squareSize / 2f);
                var randomYPos = Random.Range(-squareSize / 2f, squareSize / 2f);
                var randomPosition = new Vector3(randomXPos, randomYPos, 0);

                var symbolTransform = symbol.transform;

                symbolTransform.localPosition = randomPosition;
                symbolTransform.rotation = Quaternion.Euler(0f, 0f, randomAngle);
                symbolTransform.localScale = new Vector3(symbolsSize, symbolsSize, 1);

                var speed = Random.Range(-0.05f, 0.05f);
                symbol.Animator.SetFloat(Speed, speed);
            }
        }

        public void UpdateSymbols(Sprite texture, Color color)
        {
            foreach (var symbol in _symbols)
            {
                symbol.Texture.sprite = texture;
                symbol.Texture.color = color;
            }
        }
    }
}