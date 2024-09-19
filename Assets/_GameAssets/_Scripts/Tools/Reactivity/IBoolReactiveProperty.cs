using System;

namespace TapAndRun.Tools.Reactivity
{
    public interface IBoolReactiveProperty
    {
        bool Value { get; }
        void Subscribe(Action<bool> method, bool isNeedUpdate = false);
        void Subscribe(Action trueMethod, Action falseMethod);
        void Subscribe(Action method, bool triggerValue);
        void Unsubscribe(Action<bool> method);
        void Unsubscribe(Action method, bool triggerValue);
        void Unsubscribe(Action trueMethod, Action falseMethod);
    }
}