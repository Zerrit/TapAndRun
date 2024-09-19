using UnityEngine;

namespace TapAndRun.PrallaxBackground.MatrixBackground
{
    public class MatrixIndexator
    {
        public int CenterIndex => _indexGrid[_midIndex, _midIndex].y * _size + _indexGrid[_midIndex, _midIndex].x;

        private readonly int _size;
        private readonly int _midIndex;

        private readonly Vector2Int[,] _indexGrid;

        public MatrixIndexator(int size)
        {
            _size = size;
            _midIndex = _size / 2;
            _indexGrid = new Vector2Int[_size, _size];

            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    _indexGrid[j, i] = new Vector2Int(j, i);
                }
            }
        }

        public int GetOrderNumber(int y, int x)
        {
            return (_indexGrid[x, y].y * _size + _indexGrid[x, y].x);
        }

        /// <summary>
        /// Смещает сетку в указанном направлении согласно величине вектора.
        /// </summary>
        /// <param name="direction"></param>
        public void ShiftGrid(Vector2 direction)
        {
            for (var y = 0; y < _size; y++)
            {
                for (var x = 0; x < _size; x++)
                {
                    Vector2 value = _indexGrid[x, y];

                    value += direction;

                    if (value.x > 2)
                    {
                        value.x = 0;
                    }
                    else if (value.x < 0)
                    {
                        value.x = 2;
                    }

                    if (value.y > 2)
                    {
                        value.y = 0;
                    }
                    else if (value.y < 0)
                    {
                        value.y  = 2;
                    }

                    _indexGrid[x, y] = Vector2Int.RoundToInt(value);
                }
            }
        }
    }
}