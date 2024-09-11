using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;

namespace TapAndRun.Services.Audio
{
    public interface IAudioService: IInitializableAsync
    {
        void PlaySound(string id);
        UniTask PlaySoundAsync(string id, float delayTime, CancellationToken token);
        void CallVibration();
    }
}