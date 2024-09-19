using System;

namespace TapAndRun.Tools.Reactivity
{
    public class TriggerReactiveProperty : ITriggerReactiveProperty
    {
        public event Action OnTriggered;

        public void Trigger()
        {
            OnTriggered?.Invoke();
        }

        public void Subscribe(Action method, bool isNeedUpdate = false)
        {
            OnTriggered += method;

            if (isNeedUpdate)
            {
                OnTriggered?.Invoke();
            }
        }

        public void Unsubscribe(Action method)
        {
            OnTriggered -= method;
        }
    }
}