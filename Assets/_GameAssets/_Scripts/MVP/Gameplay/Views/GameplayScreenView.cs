using System;
using TapAndRun.UI;
using UnityEngine.EventSystems;

namespace TapAndRun.MVP.Gameplay.Views
{
    public class GameplayScreenView : ScreenView, IPointerClickHandler
    {
        public event Action OnClicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke();
        }
    }
}