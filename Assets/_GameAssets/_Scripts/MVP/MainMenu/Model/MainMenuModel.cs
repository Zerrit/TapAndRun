using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.MainMenu.Model
{
    public class SelfMainMenuModel : ISelfMainMenuModel, IMainMenuModel
    {
        public event Action OnGameStarted;

        public SimpleReactiveProperty<bool> IsDisplaying { get; private set; }

        public UniTask InitializeAsync(CancellationToken token)
        {
            IsDisplaying = new SimpleReactiveProperty<bool>();

            return UniTask.CompletedTask;
        }

        public void StartGame()
        {
            OnGameStarted?.Invoke();
        }
    }
}