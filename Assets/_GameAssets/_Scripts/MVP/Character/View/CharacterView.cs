using TapAndRun.Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace TapAndRun.MVP.Character.View
{
    public class CharacterView : MonoBehaviour
    {
        [field:SerializeField] public Transform Transform { get; private set; }
        [field:SerializeField] public Transform SkinHandler { get; private set; }
        [field:SerializeField] public Animator Animator { get; private set; }
        [field:SerializeField] public CharacterSfx Sfx { get; private set; }

        public readonly int _isMoving = Animator.StringToHash("IsMoving");
        public readonly int _isFall = Animator.StringToHash("IsFall");
        public readonly int _jump = Animator.StringToHash("Jump");
        public readonly int _speed = Animator.StringToHash("Speed");

        public void InitSkin(GameObject skin)
        {
            Animator = skin.GetComponent<Animator>();
            Sfx = skin.GetComponent<CharacterSfx>();
        }

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
            Animator.SetBool(_isMoving, isMoving);
        }

        public void UpdateFalling(bool isFall)
        {
            Animator.SetBool(_isFall, isFall);

            if (isFall)
            {
                Sfx.PlayLoseSfx();
            }
        }

        public void UpdateAnimMultiplier(float multiplier)
        {
            Animator.SetFloat(_speed, multiplier);
        }

        public void UpdateSfxAcceleration(float value)
        {
            Sfx.ChangeSpeed(value);
        }

        public void DisplayJumping()
        {
            Animator.SetTrigger(_jump);
            Sfx.PlayJumpSfx();
        }

        public void DisplayTurning()
        {
            Sfx.PlayTurnSfx();
        }
    }
}
