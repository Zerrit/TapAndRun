using System;
using TapAndRun.Interfaces;

namespace TapAndRun.Services.Update
{
    public interface ILateUpdateService
    {
        event Action OnLateUpdated;

        /// <summary>
        /// Подписка на обновление в момент Late Update.
        /// </summary>
        /// <param name="updatable"></param>
        void Subscribe(ILateUpdatable updatable);

        /// <summary>
        /// Отписка от обновления в момент LateUpdate
        /// </summary>
        /// <param name="updatable"></param>
        void Unsubscribe(ILateUpdatable updatable);
    }
}