using System;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.UI
{
    public class InfinitBackMover : MonoBehaviour
    {
        [SerializeField] private RawImage _rawImage;

        [SerializeField] private float _ySpeed;
        [SerializeField] private float _xSpeed;

        private Vector2 _uvRect;
        
        private void Update()
        {
            _uvRect.x += _xSpeed * Time.deltaTime;
            _uvRect.y += _ySpeed * Time.deltaTime;

            _rawImage.uvRect = new Rect(_uvRect, Vector2.one);
        }
    }
}