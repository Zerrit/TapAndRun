using TapAndRun.Configs;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace TapAndRun.PrallaxBackground
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _camera;

        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private ParallaxLayer[] _layers;

        [SerializeField] private BackgroundsConfig _config;

        private int _currentPresetIndex;
        private int _presetsCount;

        private void Start()
        {
            _presetsCount = _config.BackgroundsPresets.Length;
            
            SetStyle(_currentPresetIndex);
        }

        public void Update()
        {
            var backgroundPos = _transform.position;
            var cameraPos = _camera.position;
            Vector2 motionVector = cameraPos - backgroundPos;

            if (motionVector == Vector2.zero)
            {
                return;
            }
            
            var targetPos = new Vector3(cameraPos.x, cameraPos.y, backgroundPos.z);
            _transform.position = targetPos;

            foreach (var layer in _layers)
            {
                layer.Move(motionVector);
            }
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