using System;

namespace TapAndRun.Tools.Reactivity
{
    public interface ISimpleReactiveProperty<T>
    {
        event Action<T> OnChanged;

        T Value { get; }

        void Subscribe(Action<T> method);
        void SubscribeAndUpdate(Action<T> method);
        void Unsubscribe(Action<T> method);
    }
}