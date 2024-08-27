using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Audio;
using TapAndRun.Configs;
using TapAndRun.Interfaces;
using UnityEngine;
using UnityEngine.Audio;

namespace TapAndRun.Services.Audio
{
    public interface IAudioService: IInitializableAsync
    {
        void PlaySound(string id);
    }

    public interface IAudioController
    {
        void ChangeMuteStatus(bool status);
    }

    public class AudioService : MonoBehaviour, IAudioService, IAudioController
    {
        [SerializeField] private SoundConfig _soundConfig;
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioMixer _mixer;

        [SerializeField] private float _defaultVolume;
        [SerializeField] private float _muteVolume;

        private Dictionary<string, Sound> _sounds;

        public UniTask InitializeAsync(CancellationToken token)
        {
            _sounds = new Dictionary<string, Sound>();

            foreach (var sound in _soundConfig.Sounds)
            {
                _sounds[sound.SoundId] = sound;
            }
            
            return UniTask.CompletedTask;
        }
        
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

        public void PlaySound(string id)
        {
            if (!_sounds.ContainsKey(id))
            {
                Debug.Log("Запрашиваемый звук не найден");
                return;
            }
            
            _source.PlayOneShot(_sounds[id].Clip, _sounds[id].Volume);
        }
    }
}