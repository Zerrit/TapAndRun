using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Levels.View;

namespace TapAndRun.Factories
{
    public interface ILevelFactory
    {
        UniTask<LevelView> CreateLevelViewAsync(int levelId, CancellationToken token);
    }
}