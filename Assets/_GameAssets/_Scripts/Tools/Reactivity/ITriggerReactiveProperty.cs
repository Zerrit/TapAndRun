using System;

namespace TapAndRun.Tools.Reactivity
{
    public interface ITriggerReactiveProperty
    {
        void Subscribe(Action method, bool isNeedUpdate = false);
        void Unsubscribe(Action method);
    }
}