using System;

namespace TapAndRun.Tools.Reactivity
{
    public class BoolReactiveProperty : IBoolReactiveProperty
    {
        public event Action<bool> OnChanged;
        public event Action OnChangedToTrue;
        public event Action OnChangedToFalse;

        public bool Value
        {
            get => _value;
            set
            {
                _value = value;
                OnChanged?.Invoke(_value);

                if (value)
                {
                    OnChangedToTrue?.Invoke();
                }
                else
                {
                    OnChangedToFalse?.Invoke();
                }
            }
        }

        private bool _value;

        public BoolReactiveProperty(bool startValue = default)
        {
            _value = startValue;
        }

        public void Subscribe(Action<bool> method, bool isNeedUpdate = false)
        {
            OnChanged += method;

            if (isNeedUpdate)
            {
                OnChanged?.Invoke(_value);
            }
        }

        public void Subscribe(Action trueMethod, Action falseMethod)
        {
            OnChangedToTrue += trueMethod;
            OnChangedToFalse += falseMethod;
        }

        public void Subscribe(Action method, bool triggerValue)
        {
            if (triggerValue)
            {
                OnChangedToTrue += method;
            }
            else
            {
                OnChangedToFalse += method;
            }
        }

        public void Unsubscribe(Action<bool> method)
        {
            OnChanged -= method;
        }

        public void Unsubscribe(Action method, bool triggerValue)
        {
            if (triggerValue)
            {
                OnChangedToTrue -= method;
            }
            else
            {
                OnChangedToFalse -= method;
            }
        }

        public void Unsubscribe(Action trueMethod, Action falseMethod)
        {
            OnChangedToTrue -= trueMethod;
            OnChangedToFalse -= falseMethod;
        }
    }
}