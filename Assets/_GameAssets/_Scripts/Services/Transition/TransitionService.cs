using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Services.Audio;

namespace TapAndRun.Services.Transition
{
    public class TransitionService : ITransitionService, IDisposable
    {
        public bool IsScreenHidden { get; private set; }

        private bool _canFinish;

        private CancellationTokenSource _cts;

        private readonly IAudioService _audioService;
        private readonly TransitionView _view;

        public TransitionService(IAudioService audioService, TransitionView view)
        {
            _audioService = audioService;
            _view = view;
        }

        public UniTask InitializeAsync(CancellationToken token) //TODO Необходима ли инициализация?
        {
            _cts = new CancellationTokenSource();

            return UniTask.CompletedTask;
        }

        public async UniTask ShowTransition(CancellationToken token, bool canFinish = false)
        {
            _canFinish = canFinish;

            _audioService.PlaySound("SwooshIn");
            await _view.ShowAsync(token);

            IsScreenHidden = true;

            if (canFinish)
            {
                TryEndTransition();
            }
        }

        public void TryEndTransition() //TODO Продумать логику
        {
            _audioService.PlaySound("SwooshOut");
            IsScreenHidden = false;
            _view.HideAsync(_cts.Token).Forget();
        }

        public void Dispose()
        {
            _cts?.Dispose();
        }
    }
}