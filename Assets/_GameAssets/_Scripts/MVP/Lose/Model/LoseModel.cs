using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Lose.Model
{
    public class LoseModel : ISelfLoseModel, ILoseModel
    {
        public event Action OnHomeSelected;
        public event Action OnRestartSelected;
        
        public ReactiveProperty<bool> IsDisplaying { get; private set; }

        public LoseModel()
        {
        }
        
        public UniTask InitializeAsync(CancellationToken token)
        {
            IsDisplaying = new ReactiveProperty<bool>();

            return UniTask.CompletedTask;
        }

        public void Restart()
        {
            OnRestartSelected?.Invoke();
        }
        
        public void BackToHome()
        {
            OnHomeSelected?.Invoke();
        }
    }
}