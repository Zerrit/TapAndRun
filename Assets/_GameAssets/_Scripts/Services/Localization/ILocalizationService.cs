using TapAndRun.Interfaces;

namespace TapAndRun.Services.Localization
{
    public interface ILocalizationService : IInitializableAsync
    {
        void ChangeLanguage(string langId);
    }
}