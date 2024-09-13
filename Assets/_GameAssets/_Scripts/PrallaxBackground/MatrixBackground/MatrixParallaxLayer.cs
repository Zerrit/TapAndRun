using UnityEngine;

namespace TapAndRun.PrallaxBackground.MatrixBackground
{
    public class MatrixParallaxLayer : MonoBehaviour
    {
        [SerializeField] private Transform _layerParent;
        
        [SerializeField] private ParallaxSquare[] _parallaxSquares = { };

        [SerializeField] private int _layerOrder;
        [SerializeField] private float _parallaxSpeed;
        [SerializeField] private int _gridSize;
        [SerializeField] private int _squareSize;

        private ParallaxSquare _centerSquare;
        private MatrixIndexator _indexator;

        public void Initialize()
        {
            _indexator = new MatrixIndexator(_gridSize);
            _centerSquare = _parallaxSquares[_indexator.CenterIndex];

            foreach (var square in _parallaxSquares)
            {
                square.Initialize(_squareSize);
            }
        }

        public void Move(Vector2 motionVector, Vector3 targetPos)
        {
            Vector3 layerOffset = new Vector3(motionVector.x, motionVector.y, 0) * (-1 * _parallaxSpeed);
            _layerParent.position += layerOffset;

            if (Mathf.Abs(targetPos.x - _centerSquare.transform.position.x) > _squareSize / 2f)
            {
                if (targetPos.x - _centerSquare.transform.position.x > 0)
                {
                    for (int i = 0; i < _gridSize; i++)
                    {
                        _parallaxSquares[_indexator.GetOrderNumber(i, 0)].transform.Translate(Vector3.right * (3 * _squareSize), Space.Self);
                    }
                    _indexator.ShiftGrid(Vector2Int.right);
                    _centerSquare = _parallaxSquares[_indexator.CenterIndex];
                    
                    Debug.Log("Пересечение ВПРАВО");
                }
                else
                {
                    for (int i = 0; i < _gridSize; i++)
                    {
                        _parallaxSquares[_indexator.GetOrderNumber(i, 2)].transform.Translate(Vector3.left * (3 * _squareSize), Space.Self);
                    }
                    _indexator.ShiftGrid(Vector2Int.left);
                    _centerSquare = _parallaxSquares[_indexator.CenterIndex];
                    
                    Debug.Log("Пересечение ВЛЕВО");
                }
                
                return;
            }
            
            if (Mathf.Abs(targetPos.y - _centerSquare.transform.position.y) > _squareSize / 2f)
            {
                Debug.Log("Пересечение ПО ВЕРТИКАЛИ");
                if (targetPos.y - _centerSquare.transform.position.y > 0)
                {
                    for (int i = 0; i < _gridSize; i++)
                    {
                        _parallaxSquares[_indexator.GetOrderNumber(2, i)].transform.Translate(Vector3.up * (3 * _squareSize), Space.Self);
                    }
                    _indexator.ShiftGrid(Vector2Int.down);
                    _centerSquare = _parallaxSquares[_indexator.CenterIndex];
                    
                    Debug.Log("Пересечение ВВЕРХ");
                }
                else
                {
                    for (int i = 0; i < _gridSize; i++)
                    {
                        _parallaxSquares[_indexator.GetOrderNumber(0, i)].transform.Translate(Vector3.down * (3 * _squareSize), Space.Self);
                    }
                    _indexator.ShiftGrid(Vector2Int.up);
                    _centerSquare = _parallaxSquares[_indexator.CenterIndex];
                    
                    Debug.Log("Пересечение ВНИЗ");
                }
                
                return;
            }
        }

        public void SetPreset(Sprite texture, Color color)
        {
            Color.RGBToHSV(color, out var hue, out var saturation, out var brithness);

            saturation *= 1f - (_layerOrder * 0.25f);
            brithness *= 1f + (_layerOrder * 0.2f);

            var newColor = Color.HSVToRGB(hue, saturation, brithness);
            
            foreach (var square in _parallaxSquares)
            {
                square.UpdateSymbols(texture, newColor);
            }
        }
    }
}