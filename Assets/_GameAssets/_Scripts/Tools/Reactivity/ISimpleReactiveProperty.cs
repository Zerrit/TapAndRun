using System;

namespace TapAndRun.Tools.Reactivity
{
    public interface ISimpleReactiveProperty<T>
    {
        event Action<T> OnChanged;
        T Value { get; }
    }
}