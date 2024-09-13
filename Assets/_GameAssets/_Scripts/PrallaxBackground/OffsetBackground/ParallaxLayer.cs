using System;
using UnityEngine;

namespace TapAndRun.PrallaxBackground
{
    [Serializable]
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] private int _layerOrder;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private float _parallaxSpeed;

        [SerializeField] private float _alfa;
        
        [SerializeField] private bool _isAutoMoving;
        [SerializeField] private Vector2 _autoMoveDirection;
        [SerializeField] private float _autoMoveSpeed;

        private Material _mat;

        public void Start()
        {
            _mat = _renderer.material;
        }

        private void Update()
        {
            if (!_isAutoMoving)
            {
                return;
            }

            _mat.mainTextureOffset += _autoMoveDirection * (_autoMoveSpeed / 100f * Time.fixedUnscaledDeltaTime);
        }

        public void SetPreset(Sprite sprite, Color color)
        {
            _renderer.sprite = sprite;

            Color.RGBToHSV(color, out var hue, out var saturation, out var brithness);

            saturation *= 1f - (_layerOrder * 0.25f);
            brithness *= 1f + (_layerOrder * 0.2f);

            _renderer.color = Color.HSVToRGB(hue, saturation, brithness);
        }
        
        public void Move(Vector2 offsetVector)
        {
            var offset = offsetVector * (_parallaxSpeed * Time.fixedUnscaledDeltaTime);
            _mat.mainTextureOffset += offset;
        }
    }
}