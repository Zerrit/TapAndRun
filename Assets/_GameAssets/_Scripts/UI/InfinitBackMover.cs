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
        [SerializeField] private float _width;
        [SerializeField] private float _height;

        private Vector2 _uvRect;
        
        private void Update()
        {
            _uvRect.x += _xSpeed / 100 * Time.deltaTime;
            _uvRect.y += _ySpeed / 100 * Time.deltaTime;
            var size = new Vector2(_width, _height);

            _rawImage.uvRect = new Rect(_uvRect, size);
        }
    }
}