using System;
using TapAndRun.Configs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TapAndRun.MVP.Settings.Views
{
    public class LangButton : MonoBehaviour, IPointerClickHandler
    {
        public event Action<LanguageConfig> OnClicked;

        public LanguageConfig LanguageConfig { get; private set; }

        [SerializeField] private Image _icon;

        public void Initialize(LanguageConfig langConfig)
        {
            LanguageConfig = langConfig;

            _icon.sprite = LanguageConfig.Icon;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(LanguageConfig);
        }
    }
}