using System;
using TapAndRun.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TapAndRun.MVP.Levels.Views
{
    public class GameplayScreenView : ScreenView, IPointerClickHandler
    {
        public event Action OnClicked;

        [field: SerializeField] public GameplayTutorialView _tutorialView;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke();
        }
    }
}