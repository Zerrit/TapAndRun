using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.SimpleLocalization.Scripts;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using UnityEngine;

namespace TapAndRun.Services.Localization
{
    public class LocalisationService : MonoBehaviour, ILocalisationService
    {
        [SerializeField] private LanguageConfig[] _languages;

        private Dictionary<string, Sprite> _langIcons;

        public UniTask InitializeAsync(CancellationToken token)
        {
            _langIcons = new Dictionary<string, Sprite>();
            _langIcons = _languages.ToDictionary(x => x.Id, x => x.Icon);
            
            LocalizationManager.Read();
            
            return UniTask.CompletedTask;
        }

        public Sprite GetLangIcon(string id)
        {
            return _langIcons[id];
        }

        public void ChangeLanguage(string langId)
        {
            LocalizationManager.Language = langId;
        }
    }
}
