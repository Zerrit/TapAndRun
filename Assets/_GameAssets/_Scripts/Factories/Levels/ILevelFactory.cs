using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Gameplay.Views;
using UnityEngine;

namespace TapAndRun.Factories.Levels
{
    public interface ILevelFactory
    {
        public UniTask<LevelView> CreateLevelViewAsync(int levelId, Vector2 position, Quaternion rotation, CancellationToken token);
        public void Dispose();
    }
}