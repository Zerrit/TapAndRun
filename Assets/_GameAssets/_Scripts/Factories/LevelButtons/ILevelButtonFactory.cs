using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Screens.LevelSelect;
using UnityEngine;

namespace TapAndRun.Factories.LevelButtons
{
    public interface ILevelButtonFactory
    {
        UniTask<LevelButtonView> CreateAsynс(Transform parent, CancellationToken token);
        void Dispose();
    }
}