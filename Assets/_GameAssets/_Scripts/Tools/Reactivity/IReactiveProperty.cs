using System;

namespace TapAndRun.Tools.Reactivity
{
    public interface IReactiveProperty<T>
    {
        T Value { get; }

        void Subscribe(Action<T> method, bool isNeedUpdate = false);
        void Unsubscribe(Action<T> method);
    }
}