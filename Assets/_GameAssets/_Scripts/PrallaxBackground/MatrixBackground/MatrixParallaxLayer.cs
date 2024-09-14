using System;
using UnityEngine;

namespace TapAndRun.PrallaxBackground.MatrixBackground
{
    public class MatrixParallaxLayer : MonoBehaviour
    {
        [SerializeField] private Transform _layerParent;

        [SerializeField, Header("Количество элементов должно быть квадратом gridSize")] 
        private ParallaxSquare[] _parallaxSquares = { };

        [SerializeField,Header("Размер стороны ячейки")] 
        private int _squareSize;

        [SerializeField] private int _saturationOffset;
        [SerializeField] private int _brithnessOffset;
        [SerializeField] private float _parallaxSpeed;

        [SerializeField, Header("Пассивное движение")]
        private bool _isAutoMove;
        [SerializeField] private Vector2 _autoMoveDirection;

        private ParallaxSquare _centerSquare;
        private MatrixIndexator _indexator;
        private Transform _target;
        
        private const int GridSize = 3;

        public void Update()
        {
            if (!_isAutoMove)
            {
                return;
            }
            
            Move(_autoMoveDirection);
        }

        public void Initialize(Transform target)
        {
            _target = target;
            _indexator = new MatrixIndexator(GridSize);
            _centerSquare = _parallaxSquares[_indexator.CenterIndex];

            BuildSquareGrid();

            foreach (var square in _parallaxSquares)
            {
                square.Initialize(_squareSize);
            }
        }

        public void SetPreset(Sprite texture, Color color)
        {
            Color.RGBToHSV(color, out var hue, out var saturation, out var brithness);

            saturation += _saturationOffset * 0.01f;
            brithness += _brithnessOffset * 0.01f;

            var newColor = Color.HSVToRGB(hue, saturation, brithness);
            
            foreach (var square in _parallaxSquares)
            {
                square.UpdateSymbols(texture, newColor);
            }
        }
        
        public void Move(Vector2 motionVector)
        {
            var layerOffset = new Vector3(motionVector.x, motionVector.y, 0) * (-1 * _parallaxSpeed);
            _layerParent.position += layerOffset;

            UpdateGrid();
        }

        private void UpdateGrid()
        {
            var targetPos = _target.position;
            
            if (Mathf.Abs(targetPos.x - _centerSquare.transform.position.x) > _squareSize / 2f)
            {
                if (targetPos.x - _centerSquare.transform.position.x > 0)
                {
                    for (var i = 0; i < GridSize; i++)
                    {
                        _parallaxSquares[_indexator.GetOrderNumber(i, 0)].transform.Translate(Vector3.right * (3 * _squareSize), Space.Self);
                    }
                    _indexator.ShiftGrid(Vector2Int.right);
                    _centerSquare = _parallaxSquares[_indexator.CenterIndex];
                }
                else
                {
                    for (var i = 0; i < GridSize; i++)
                    {
                        _parallaxSquares[_indexator.GetOrderNumber(i, 2)].transform.Translate(Vector3.left * (3 * _squareSize), Space.Self);
                    }
                    _indexator.ShiftGrid(Vector2Int.left);
                    _centerSquare = _parallaxSquares[_indexator.CenterIndex];
                }
                
                return;
            }
            
            if (Mathf.Abs(targetPos.y - _centerSquare.transform.position.y) > _squareSize / 2f)
            {
                if (targetPos.y - _centerSquare.transform.position.y > 0)
                {
                    for (var i = 0; i < GridSize; i++)
                    {
                        _parallaxSquares[_indexator.GetOrderNumber(2, i)].transform.Translate(Vector3.up * (3 * _squareSize), Space.Self);
                    }
                    _indexator.ShiftGrid(Vector2Int.down);
                    _centerSquare = _parallaxSquares[_indexator.CenterIndex];
                }
                else
                {
                    for (var i = 0; i < GridSize; i++)
                    {
                        _parallaxSquares[_indexator.GetOrderNumber(0, i)].transform.Translate(Vector3.down * (3 * _squareSize), Space.Self);
                    }
                    _indexator.ShiftGrid(Vector2Int.up);
                    _centerSquare = _parallaxSquares[_indexator.CenterIndex];
                }
            }
        }

        private void BuildSquareGrid()
        {
            var index = 0;
            
            for (int y = 1; y > -2; y--)
            {
                for (int x = -1; x < 2; x++)
                {
                    _parallaxSquares[index].transform.localPosition = new Vector3(_squareSize * x, _squareSize * y, 0);
                    index++;
                }
            }
        }
    }
}