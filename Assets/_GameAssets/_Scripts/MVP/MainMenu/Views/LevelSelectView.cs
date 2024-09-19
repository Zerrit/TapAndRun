using System.Collections.Generic;
using TapAndRun.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.MVP.MainMenu.Views
{
    public class LevelSelectView : PopupView
    {
        [field:SerializeField] public Button BackButton { get; private set; }
        [field:SerializeField] public Transform ButtonsContainer { get; private set; }

        public List<LevelButtonView> ButtonList { get; }= new();

        public void UpdateButtons(int lastUnlockedLevelId)
        {
            foreach (var button in ButtonList)
            {
                if (button.LevelId <= lastUnlockedLevelId)
                {
                    button.Unlock();
                }
                else
                {
                    button.Lock();
                }
            }
        }
    }
}