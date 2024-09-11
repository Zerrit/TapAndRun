using TapAndRun.Audio;
using UnityEngine;

namespace TapAndRun.MVP.Character.View
{
    public class CharacterView : MonoBehaviour
    {
        [field:SerializeField] public Transform Transform { get; private set; }
        //[field:SerializeField] public Transform RoadChecker{ get; private set; }

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

            Sfx.SwitchRunSfx(isMoving);
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
            ActivateAnimation(_jump);
            Sfx.StopRunSfx();
            Sfx.PlayJumpSfx();
        }

        public void DisplayEndJumping()
        {
            Sfx.PlayRunSfx();
        }

        public void DisplayTurning()
        {
            Sfx.PlayTurnSfx();
        }
        
        public void ActivateAnimation(int animHash)
        {
            Animator.SetTrigger(animHash);
        }
    }
}
