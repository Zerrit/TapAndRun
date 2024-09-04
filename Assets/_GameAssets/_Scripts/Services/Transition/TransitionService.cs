using System.Threading;
using Cysharp.Threading.Tasks;

namespace TapAndRun.Services.Transition
{
    public class TransitionService : ITransitionService
    {
        public bool IsScreenHidden { get; private set; }

        private bool _canFinish;

        private CancellationTokenSource _cts;

        private readonly TransitionView _view;

        public TransitionService(TransitionView view)
        {
            _view = view;
        }

        public UniTask InitializeAsync(CancellationToken token) //TODO Необходима ли инициализация?
        {
            _cts = new CancellationTokenSource();

            return UniTask.CompletedTask;
        }

        public async UniTask PlayTransition(CancellationToken token, bool canFinish = false)
        {
            _canFinish = canFinish;

            await _view.ShowAsync(token);

            IsScreenHidden = true;
            
            //await UniTask.WaitUntil(() => _canFinish, cancellationToken: token);

            if (canFinish)
            {
                TryEndTransition();
            }
        }

        public void TryEndTransition()
        {
            IsScreenHidden = false;
            _view.HideAsync(_cts.Token).Forget();
        }

        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}