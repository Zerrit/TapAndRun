﻿using TapAndRun.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TapAndRun.MVP.Screens.Lose
{
    public class LoseScreenView : ScreenView
    {
        [field:SerializeField] public Button RestartButton { get; private set; }

        public override void Show()
        {
            base.Show();
            
            
        }

        public override void Hide()
        {
            base.Hide();
            
            
        }
    }
}