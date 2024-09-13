using UnityEngine;

namespace TapAndRun.PrallaxBackground.MatrixBackground
{
    public class ParallaxSquare : MonoBehaviour
    {
        [SerializeField] private BackgroundSymbol[] _symbols;

        public void Initialize(float squareSize)
        {
            foreach (var symbol in _symbols)
            {
                var randomAngle = Random.Range(0f, 360f);
                var randomXPos = Random.Range(-squareSize / 2f, squareSize / 2f);
                var randomYPos = Random.Range(-squareSize / 2f, squareSize / 2f);
                var randomPosition = new Vector3(randomXPos, randomYPos, 0);

                symbol.transform.localPosition = randomPosition;
                symbol.transform.rotation = Quaternion.Euler(0f, 0f, randomAngle);
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