using System;
using TapAndRun.Interfaces;
using TapAndRun.Tools;
using UnityEngine;

namespace TapAndRun.Services.Update
{
    public class UpdateService : MonoBehaviour, IUpdateService, ILateUpdateService
    {
        public event Action OnUpdated;
        public event Action OnLateUpdated;

        // UPDATE SUBSCRIPTION //
        
        public void Subscribe(IUpdatable updatable)
        {
            OnUpdated += updatable.Update;
        }

        public void Unsubscribe(IUpdatable updatable)
        {
            OnUpdated -= updatable.Update;
        }

        // LATE UPDATE SUBSCRIPTION //
        
        public void Subscribe(ILateUpdatable updatable)
        {
            OnLateUpdated += updatable.LateUpdate;
        }

        public void Unsubscribe(ILateUpdatable updatable)
        {
            OnLateUpdated -= updatable.LateUpdate;
        }

        private void Update()
        {
            OnUpdated?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdated?.Invoke();
        }
    }
}