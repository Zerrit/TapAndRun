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
    public class AudioService : MonoBehaviour, IAudioService, IDecomposable
    {
        public AudioMixerGroup MixerGroup => _source.outputAudioMixerGroup;
        
        [SerializeField] private float _defaultVolume;
        [SerializeField] private SoundConfig _soundConfig;
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioMixer _mixer;

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

            _settingsModel.AudioStatus.Subscribe(SetAudioState, true);
            _settingsModel.VibroStatus.Subscribe(SetVibroState, true);

            return UniTask.CompletedTask;
        }

        public void PlaySound(string id)
        {
            if (!_sounds.ContainsKey(id))
            {
                Debug.Log("Requested sound was not found");
                return;
            }

            _source.PlayOneShot(_sounds[id].Clip, _sounds[id].Volume);
        }

        public async UniTask PlaySoundAsync(string id, float delayTime, CancellationToken token)
        {
            if (!_sounds.ContainsKey(id))
            {
                Debug.Log("Requested sound was not found");
                return;
            }

            await UniTask.WaitForSeconds(delayTime, cancellationToken: token);
            
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

        private void SetAudioState(bool state)
        {
            if (state)
            {
                _mixer.SetFloat("Volume", CalculateVolume(_defaultVolume));
            }
            else
            {
                _mixer.SetFloat("Volume", CalculateVolume(MuteVolume));
            }
        }

        private void SetVibroState(bool state)
        {
            _isVibroActive = state;

            if (state)
            {
                CallVibration();
            }
        }

        private float CalculateVolume(float value)
        {
            return Mathf.Log10(value) * 20;
        }

        public void Decompose()
        {
            _settingsModel.AudioStatus.Unsubscribe(SetAudioState);
            _settingsModel.VibroStatus.Unsubscribe(SetVibroState);
        }
    }
}