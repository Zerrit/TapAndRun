using TapAndRun.Audio;
using UnityEngine;

namespace TapAndRun.MVP.Character.View
{
    public class CharacterView : MonoBehaviour
    {
        [field:SerializeField] public Transform Transform { get; private set; }
        [field:SerializeField] public Transform RoadChecker{ get; private set; }

        [field:SerializeField] public Transform SkinHandler { get; private set; }
        [field:SerializeField] public Animator Animator { get; private set; }
        [field:SerializeField] public CharacterSfx Sfx { get; private set; }

        public readonly int IsMoving = Animator.StringToHash("IsMoving");
        public readonly int IsFall = Animator.StringToHash("IsFall");
        public readonly int Jump = Animator.StringToHash("Jump");
        public readonly int Speed = Animator.StringToHash("Speed");

        public void UpdatePosition(Vector3 position)
        {
            Transform.position = position;
        }

        public void UpdateRotation(float rotation)
        {
            var rotationEuler = Quaternion.Euler(0f, 0f, rotation);
            Transform.rotation = rotationEuler;
        }

        public void UpdateMoving(bool isMoving)
        {
            Animator.SetBool(IsMoving, isMoving);

            Sfx.SwitchRunSfx(isMoving);
        }

        public void UpdateFalling(bool isFall)
        {
            Animator.SetBool(IsFall, isFall);

            if (isFall)
            {
                Sfx.PlayLoseSfx();
            }
        }

        public void UpdateAnimMultiplier(float multiplier)
        {
            Animator.SetFloat(Speed, multiplier);
        }
        
        public void ActivateAnimation(int animHash)
        {
            Animator.SetTrigger(animHash);
        }
    }
}
