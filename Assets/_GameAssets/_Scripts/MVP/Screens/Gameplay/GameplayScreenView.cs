using System;
using TapAndRun.UI;
using UnityEngine.EventSystems;

namespace TapAndRun.MVP.Screens.Gameplay
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