using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace TapAndRun.Audio
{
    public class CharacterSfx : MonoBehaviour
    {
        [SerializeField] private AudioClip _firstStepSound;
        [SerializeField] private AudioClip _turnSound;
        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private AudioClip _loseSound;

        [SerializeField] private float _baseSpeedPitch;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _runAudioSource;

        public void SetMixer(AudioMixerGroup mixerGroup)
        {
            _audioSource.outputAudioMixerGroup = mixerGroup;
            _runAudioSource.outputAudioMixerGroup = mixerGroup;
        }

        public void ChangeSpeed(float acceleration)
        {
            _audioSource.pitch = _baseSpeedPitch + acceleration;
        }

        public void PlayStepSfx()
        {
            _runAudioSource.pitch = Random.Range(0.9f, 1.25f);
            _runAudioSource.PlayOneShot(_firstStepSound);
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
