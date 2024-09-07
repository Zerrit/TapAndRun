using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.MainMenu.Views;
using UnityEngine;

namespace TapAndRun.Factories.LevelButtons
{
    public interface ILevelButtonFactory
    {
        UniTask<LevelButtonView> CreateAsynс(Transform parent, CancellationToken token);
        void Decompose();
    }
}