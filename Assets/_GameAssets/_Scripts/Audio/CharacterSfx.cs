using UnityEngine;

namespace TapAndRun.Audio
{
    public class CharacterSfx : MonoBehaviour
    {
        [SerializeField] private AudioClip _firstStepSound;
        [SerializeField] private AudioClip _secondStepSound;
        [SerializeField] private AudioClip _turnSound;
        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private AudioClip _loseSound;

        [SerializeField] private float _baseSpeedPitch;
        
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _runAudioSource;

        public void ChangeSpeed(float acceleration)
        {
            _audioSource.pitch = _baseSpeedPitch + acceleration;
        }

        public void PlayFirstStepSfx()
        {
            _runAudioSource.pitch = Random.Range(0.95f, 1.2f);
            _runAudioSource.PlayOneShot(_firstStepSound);
        }

        public void PlaySecondStepSfx()
        {
            _runAudioSource.pitch = Random.Range(0.95f, 1.2f);
            _runAudioSource.PlayOneShot(_secondStepSound);
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
