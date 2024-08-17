using UnityEngine;
using UnityEngine.Audio;

namespace TapAndRun.Services
{
    public interface IAudioService
    {
        void ChangeMuteStatus(bool status);
    }

    public class AudioService : MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioMixer _mixer;

        [SerializeField] private float _defaultVolume;
        [SerializeField] private float _muteVolume;

        public void ChangeMuteStatus(bool status)
        {
            if (status)
            {
                _mixer.SetFloat("Volume", _defaultVolume);
            }
            else
            {
                _mixer.SetFloat("Volume", _muteVolume);
            }
        }
    }
}