using System;
using UnityEngine;

namespace TapAndRun.Character.View
{
    public class CharacterView : MonoBehaviour
    {
        public event Action OnLevelStarted;
        public event Action OnLevelFinished;
        public event Action OnCrystalTaken;

        [field:SerializeField] public Transform CharacterTransform { get; private set; }
        [field:SerializeField] public Transform SkinHandler { get; private set; }
        [field:SerializeField] public Transform RoadChecker { get; private set; }
        [field:SerializeField] public Animator CharacterAnimator { get; private set; }

        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Jump = Animator.StringToHash("Jump");

        public void ActiveRunAnimation()
        {
            CharacterAnimator.SetTrigger(Run);
        }

        public void ActiveJumpAnimation()
        {
            CharacterAnimator.SetTrigger(Jump);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Finish"))
            {
                OnLevelFinished?.Invoke();
            }

            if (collision.CompareTag("Start"))
            {
                OnLevelStarted?.Invoke();
            }

            if (collision.CompareTag("Crystal"))
            {
                OnCrystalTaken?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Start"))
            {
                //isCanTapping = true;
            }
        }
    }
}
