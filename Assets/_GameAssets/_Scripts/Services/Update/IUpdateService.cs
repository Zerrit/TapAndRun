using System;
using TapAndRun.Interfaces;
using TapAndRun.Tools;

namespace TapAndRun.Services.Update
{
    public interface IUpdateService
    {
        event Action OnUpdated;

        void Subscribe(IUpdatable updatable);
        void Unsubscribe(IUpdatable updatable);
    }
}