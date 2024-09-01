using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.SimpleLocalization.Scripts;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using TapAndRun.MVP.Settings.Model;
using UnityEngine;

namespace TapAndRun.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private readonly ISettingsModel _settingsModel;

        public LocalizationService(ISettingsModel settingsModel)
        {
            _settingsModel = settingsModel;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            LocalizationManager.Read();

            _settingsModel.Language.Subscribe(ChangeLanguage, true);

            return UniTask.CompletedTask;
        }

        public void ChangeLanguage(string langId)
        {
            LocalizationManager.Language = langId;
            Debug.Log($"Установлен язык: {langId}");
        }
    }
}
