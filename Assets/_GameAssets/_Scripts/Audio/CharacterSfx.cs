using System;
using UnityEngine;

namespace TapAndRun.Audio
{
    public class CharacterSfx : MonoBehaviour
    {
        [SerializeField] private AudioClip _runSound;
        [SerializeField] private AudioClip _turnSound;
        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private AudioClip _loseSound;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _baseSpeedPitch;

        private void Awake()
        {
            _audioSource.clip = _runSound;
        }

        public void ChangeSpeed(int speed)
        {
            _audioSource.pitch = _baseSpeedPitch + (0.1f * speed);
        }
        
        public void PlayRunSfx()
        {
            _audioSource.Play();
        }

        public void PlayTurnSfx()
        {
            _audioSource.PlayOneShot(_turnSound);
        }

        public void PlayJumpSfx()
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(_jumpSound);
        }

        public void PlayLoseSfx()
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(_loseSound);
        }
    }
}
