using System;
using TapAndRun.Configs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TapAndRun.MVP.Settings.Views
{
    public class LangButton : MonoBehaviour, IPointerClickHandler
    {
        public event Action<string> OnClicked;

        [field:SerializeField] public Image Icon { get; private set; }

        private string _langId;

        public void Initialize(LanguageConfig langConfig)
        {
            _langId = langConfig.Id;
            Icon.sprite = langConfig.Icon;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(_langId);
        }
    }
}