﻿using System;
using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.MainMenu.Model
{
    public interface IMainMenuModel : IInitializableAsync
    {
        event Action OnGameStarted;
        
        ReactiveProperty<bool> IsDisplaying { get; }
    }
}