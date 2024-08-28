using System;

namespace TapAndRun.Tools.Reactivity
{
    public interface IReactiveProperty<T>
    {
        event Action<T> OnChanged;

        T Value { get; }

        void Subscribe(Action<T> method, bool isNeedUpdate = false);
        void Unsubscribe(Action<T> method);
    }
}