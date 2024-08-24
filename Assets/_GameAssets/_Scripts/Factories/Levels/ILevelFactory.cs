using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Levels.Views;
using UnityEngine;

namespace TapAndRun.Factories.Levels
{
    public interface ILevelFactory
    {
        int GetLevelCount();
        UniTask<LevelView> CreateLevelViewAsync(int levelId, Vector2 position, Quaternion rotation, CancellationToken token);
        void Dispose();
    }
}