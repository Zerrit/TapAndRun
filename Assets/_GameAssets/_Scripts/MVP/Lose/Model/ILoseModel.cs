using System;
using TapAndRun.Interfaces;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Lose.Model
{
    public interface ILoseModel : IInitializableAsync
    {
        event Action OnHomeSelected;
        event Action OnRestartSelected;
        
        public SimpleReactiveProperty<bool> IsDisplaying { get; }
    }
}