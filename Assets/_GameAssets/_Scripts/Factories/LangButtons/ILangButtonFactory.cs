using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.MainMenu.Views;
using TapAndRun.MVP.Settings.Views;
using UnityEngine;

namespace TapAndRun.Factories.LangButtons
{
    public interface ILangButtonFactory
    {
        int GetLangCount();

        UniTask<LangButton> CreateAsynс(int index, Transform parent, CancellationToken token);
        void Decompose();
    }
}