using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using UnityEngine.Audio;

namespace TapAndRun.Services.Audio
{
    public interface IAudioService: IInitializableAsync
    {
        public AudioMixerGroup MixerGroup { get; }

        void PlaySound(string id);
        UniTask PlaySoundAsync(string id, float delayTime, CancellationToken token);
        void CallVibration();
    }
}