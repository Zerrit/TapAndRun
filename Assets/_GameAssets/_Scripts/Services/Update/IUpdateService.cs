using System;
using TapAndRun.Interfaces;
using TapAndRun.Tools;

namespace TapAndRun.Services.Update
{
    public interface IUpdateService
    {
        event Action OnUpdated;

        /// <summary>
        /// Подписка на обновление в момент Update.
        /// </summary>
        /// <param name="updatable"></param>
        void Subscribe(IUpdatable updatable);

        /// <summary>
        /// Отписка от обновления в момент Update.
        /// </summary>
        /// <param name="updatable"></param>
        void Unsubscribe(IUpdatable updatable);
    }
}