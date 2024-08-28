using TapAndRun.Interfaces;
using UnityEngine;

namespace TapAndRun.Services.Localization
{
    public interface ILocalisationService : IInitializableAsync
    {
        Sprite GetLangIcon(string id);
        void ChangeLanguage(string langId);
    }
}