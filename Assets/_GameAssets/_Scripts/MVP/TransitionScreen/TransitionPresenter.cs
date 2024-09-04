using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.MVP.TransitionScreen.Model;

namespace TapAndRun.MVP.TransitionScreen
{
    public class TransitionPresenter : IInitializableAsync, IDisposable
    {
        private readonly ISelfTransitionModel _model;
        private readonly TransitionView _view;

        private CancellationTokenSource _cts;

        public TransitionPresenter(ISelfTransitionModel model, TransitionView view)
        {
            _model = model;
            _view = view;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();

            _model.StartTrigger.OnTriggered += MakeTransition;

            return UniTask.CompletedTask;
        }

        private void MakeTransition()
        {
            MakeTransitionAsync(_cts.Token).Forget();
            
            async UniTaskVoid MakeTransitionAsync(CancellationToken token)
            {
                await _view.ShowAsync(token);

                _model.IsScreenHidden = true;

                await UniTask.WaitUntil(() => _model.CanFinish, cancellationToken: token);

                _model.IsScreenHidden = false;

                await _view.HideAsync(token);
            }
        }

        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}