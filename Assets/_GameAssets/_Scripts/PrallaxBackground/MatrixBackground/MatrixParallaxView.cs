using TapAndRun.Configs;
using UnityEngine;

namespace TapAndRun.PrallaxBackground.MatrixBackground
{
    public class MatrixParallaxView : MonoBehaviour, IParallaxView
    {
        [SerializeField, Header("Self Transform")] 
        private Transform _transform;

        [SerializeField, Header("Character Transfrom")] 
        private Transform _target;

        [SerializeField, Header("Color Background")] 
        private SpriteRenderer _background;

        [SerializeField, Header("Parallax Layers")] 
        private MatrixParallaxLayer[] _layers;

        [SerializeField] private BackgroundsConfig _config;

        private int _currentPresetIndex;
        private int _presetsCount;

        private void Start()
        {
            _presetsCount = _config.BackgroundsPresets.Length;

            foreach (var layer in _layers)
            {
                layer.Initialize(_target);
            }

            SetStyle(_currentPresetIndex);
        }

        public void Update()
        {
            var backgroundPos = _transform.position;
            var targetPos = _target.position;
            Vector2 motionVector = targetPos - backgroundPos;

            if (motionVector == Vector2.zero)
            {
                return;
            }

            var newPosition = new Vector3(targetPos.x, targetPos.y, backgroundPos.z);
            _transform.position = newPosition;

            foreach (var layer in _layers)
            {
                layer.Move(motionVector);
            }
        }

        public void SetDefault()
        {
            _currentPresetIndex = 0;
            SetStyle(_currentPresetIndex);
        }

        public void ChangeStyle()
        {
            if (_currentPresetIndex == _presetsCount - 1)
            {
                _currentPresetIndex = 0;
            }
            else
            {
                _currentPresetIndex++;
            }

            SetStyle(_currentPresetIndex);
        }

        private void SetStyle(int index)
        {
            var preset = _config.BackgroundsPresets[index];

            _background.color = preset.Color;

            foreach (var layer in _layers)
            {
                layer.SetPreset(preset.Texture, preset.Color);
            }
        }
    }
}