using System;
using TapAndRun.UI;
using UnityEngine.EventSystems;

namespace TapAndRun.MVP.Screens.Level
{
    public class LevelScreenView : ScreenView, IPointerClickHandler
    {
        public event Action OnClicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke();
        }
    }
}