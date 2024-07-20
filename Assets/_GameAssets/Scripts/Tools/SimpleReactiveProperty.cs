using System;

namespace TapAndRun.Tools
{
    public class SimpleReactiveProperty<T>
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

        public SimpleReactiveProperty(T startValue)
        {
            _value = startValue;
        }

    }
}