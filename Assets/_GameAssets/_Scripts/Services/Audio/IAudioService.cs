using TapAndRun.Interfaces;

namespace TapAndRun.Services.Audio
{
    public interface IAudioService: IInitializableAsync
    {
        void PlaySound(string id);
        void CallVibration();
    }
}