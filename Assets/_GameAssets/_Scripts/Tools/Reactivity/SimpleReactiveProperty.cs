using System;

namespace TapAndRun.Tools.Reactivity
{
    public class SimpleReactiveProperty<T> : ISimpleReactiveProperty<T>
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

        public SimpleReactiveProperty(T startValue = default)
        {
            _value = startValue;
        }
        
        public void Subscribe(Action<T> method)
        {
            OnChanged += method;
        }

        public void SubscribeAndUpdate(Action<T> method)
        {
            OnChanged += method;
            
            OnChanged?.Invoke(_value);
        }
        
        public void Unsubscribe(Action<T> method)
        {
            OnChanged -= method;
        }
    }
}