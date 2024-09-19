using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.Services.Audio;

namespace TapAndRun.Services.Transition
{
    public class TransitionService : ITransitionService, IDecomposable
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

        public UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();
            IsScreenHidden = true;

            return UniTask.CompletedTask;
        }

        public async UniTask ShowTransition(CancellationToken token, bool canFinish = false)
        {
            _canFinish = canFinish;

            _audioService.PlaySound("SwooshIn");
            await _view.ShowAsync(token);

            IsScreenHidden = true;

            if (_canFinish)
            {
                TryEndTransition();
            }
        }

        public void TryEndTransition()
        {
            if (!IsScreenHidden)
            {
                return;
            }

            _audioService.PlaySound("SwooshOut");
            IsScreenHidden = false;
            _view.HideAsync(_cts.Token).Forget();
        }

        public void Decompose()
        {
            _cts?.Dispose();
        }
    }
}