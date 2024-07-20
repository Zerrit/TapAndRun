using UnityEngine;

namespace TapAndRun
{
    public class CrystalView : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                gameObject.SetActive(false);
            }
        }

        public void Reset()
        {
            gameObject.SetActive(true);
        }
    }
}
