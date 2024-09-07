using System.Threading;
using Cysharp.Threading.Tasks;

namespace TapAndRun.Interfaces
{
    public interface IInitializableAsync
    {
        //TODO int PriorityIndex { get; } Свойство для опеределения приоритета инициализации сущностей
        UniTask InitializeAsync(CancellationToken token);
    }
}