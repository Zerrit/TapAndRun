using System;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Views.Tutorial
{
    public class TutorialInteractPoint : MonoBehaviour
    {
        public event Action OnPlayerEntered;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerEntered?.Invoke();
            }
        }
    }
}