using UnityEngine;

namespace TapAndRun.Audio
{
    public class CharacterSfx : MonoBehaviour
    {
        [SerializeField] private AudioClip _runSound;
        [SerializeField] private AudioClip _turnSound;
        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private AudioClip _loseSound;

        [SerializeField] private float _baseSpeedPitch;
        [SerializeField] private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource.clip = _runSound;
        }

        public void ChangeSpeed(float acceleration)
        {
            _audioSource.pitch = _baseSpeedPitch + acceleration;
        }

        public void SwitchRunSfx(bool isOn)
        {
            if (isOn)
            {
                PlayRunSfx();
            }
            else
            {
                StopRunSfx();
            }
        }

        public void PlayRunSfx()
        {
            _audioSource.Play();
        }

        public void StopRunSfx()
        {
            _audioSource.Stop();
        }

        public void PlayTurnSfx()
        {
            _audioSource.PlayOneShot(_turnSound);
        }

        public void PlayJumpSfx()
        {
            _audioSource.PlayOneShot(_jumpSound);
        }

        public void PlayLoseSfx()
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(_loseSound);
        }
    }
}
