using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Audio;
using TapAndRun.Configs;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Settings.Model;
using UnityEngine;
using UnityEngine.Audio;
using VContainer;

namespace TapAndRun.Services.Audio
{
    public interface IAudioService: IInitializableAsync
    {
        void PlaySound(string id);
        void CallVibration();
    }

    public class AudioService : MonoBehaviour, IAudioService, IDisposable
    {
        [SerializeField] private SoundConfig _soundConfig;
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioMixer _mixer;

        [SerializeField] private float _defaultVolume;

        private bool _isVibroActive;
        private Dictionary<string, Sound> _sounds;

        private ISettingsModel _settingsModel;

        private const float MuteVolume = 0.0001f;

        [Inject]
        public void Construct(ISettingsModel settingsModel)
        {
            _settingsModel = settingsModel;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            _sounds = new Dictionary<string, Sound>();

            foreach (var sound in _soundConfig.Sounds)
            {
                _sounds[sound.SoundId] = sound;
            }

            _settingsModel.AudioStatus.Subscribe(SetAudioStatus, true);
            _settingsModel.VibroStatus.Subscribe(SetVibroStatus, true);
            
            return UniTask.CompletedTask;
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

        public void CallVibration()
        {
            if (!_isVibroActive)
            {
                return;
            }

            Handheld.Vibrate();
        }

        private void SetAudioStatus(bool status)
        {
            if (status)
            {
                _mixer.SetFloat("Volume", CalculateVolume(_defaultVolume));
            }
            else
            {
                _mixer.SetFloat("Volume", CalculateVolume(MuteVolume));
            }
        }

        private void SetVibroStatus(bool status)
        {
            _isVibroActive = status;

            if (status)
            {
                CallVibration();
            }
        }

        private float CalculateVolume(float value)
        {
            return Mathf.Log10(value) * 20;
        }
        
        public void Dispose()
        {
            _settingsModel.AudioStatus.Unsubscribe(SetAudioStatus);
            _settingsModel.VibroStatus.Unsubscribe(SetVibroStatus);
        }
    }
}