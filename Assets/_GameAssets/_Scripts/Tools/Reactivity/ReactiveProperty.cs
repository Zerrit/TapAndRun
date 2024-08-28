using System;

namespace TapAndRun.Tools.Reactivity
{
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        public event Action<T> OnChanged;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnChanged?.Invoke(_value);
            }
        }

        private T _value;

        public ReactiveProperty(T startValue = default)
        {
            _value = startValue;
        }
        
        public void Subscribe(Action<T> method, bool isNeedUpdate = false)
        {
            OnChanged += method;

            if (isNeedUpdate)
            {
                OnChanged?.Invoke(_value);
            }
        }

        public void Unsubscribe(Action<T> method)
        {
            OnChanged -= method;
        }
    }
}