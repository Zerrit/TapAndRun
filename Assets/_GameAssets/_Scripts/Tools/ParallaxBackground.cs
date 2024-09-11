using System;
using System.Collections.Generic;
using UnityEngine;

namespace TapAndRun.Tools
{
    [Serializable]
    public class ParallaxLayer
    {
        public SpriteRenderer _renderer;
        public float _parallaxSpeed;
    }
    
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private ParallaxLayer[] _layers;

        private List<Material> _parallaxList = new();

        [SerializeField]
        protected bool _resetPositionToZero = true;

        protected void Start()
        {
            foreach (var layer in _layers)
            {
                _parallaxList.Add(layer._renderer.material);
            }
        }

        protected void LateUpdate()
        {
            var backgroundPos = transform.position;
            var cameraPos = _camera.position;
            Vector2 moveDirection = cameraPos - backgroundPos;
            var targetPos = new Vector3(cameraPos.x, cameraPos.y, backgroundPos.z);

            transform.position = targetPos;

            for (int i = 0; i < _layers.Length; i++)
            {
                var addition = moveDirection * (_layers[i]._parallaxSpeed * Time.deltaTime);
                _parallaxList[i].mainTextureOffset += addition;
            }
        }
    }
}