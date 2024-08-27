using System;
using TapAndRun.Interfaces;

namespace TapAndRun.Services.Update
{
    public interface ILateUpdateService
    {
        event Action OnLateUpdated;

        void Subscribe(ILateUpdatable updatable);
        void Unsubscribe(ILateUpdatable updatable);
    }
}