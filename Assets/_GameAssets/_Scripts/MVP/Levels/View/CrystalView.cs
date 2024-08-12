using System;
using UnityEngine;

namespace TapAndRun.MVP.Levels.View
{
    public class CrystalView : MonoBehaviour
    {
        public event Action OnTaken;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                OnTaken?.Invoke();

                gameObject.SetActive(false);
            }
        }

        public void Reset()
        {
            gameObject.SetActive(true);
        }
    }
}
