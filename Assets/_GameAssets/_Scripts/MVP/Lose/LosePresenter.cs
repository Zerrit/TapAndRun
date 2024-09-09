using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Lose.Model;
using TapAndRun.MVP.Lose.View;
using TapAndRun.Services.Audio;

namespace TapAndRun.MVP.Lose
{
    public class LosePresenter : IInitializableAsync, IDecomposable
    {
        private readonly ISelfLoseModel _model;
        private readonly IAudioService _audioService;
        private readonly LoseView _view;

        public LosePresenter(ISelfLoseModel model, IAudioService audioService, LoseView view)
        {
            _model = model;
            _audioService = audioService;
            _view = view;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            _model.IsDisplaying.OnChanged += UpdateDisplaying;

            _view.RestartButton.onClick.AddListener(Restart);
            _view.HomeButton.onClick.AddListener(Home);

            return UniTask.CompletedTask;
        }

        private void UpdateDisplaying(bool status)
        {
            if (status)
            {
                _model.IncreaseLoseCount();
                _view.Show();
            }
            else
            {
                _view.Hide();
            }
        }

        private void Restart()
        {
            _audioService.PlaySound("Play");
            _model.RestartTrigger.Trigger();
        }

        private void Home()
        {
            _audioService.PlaySound("Button");
            _model.HomeTrigger.Trigger();
        }
        
        public void Decompose()
        {
            _model.IsDisplaying.OnChanged -= UpdateDisplaying;

            _view.RestartButton.onClick.RemoveListener(Restart);
            _view.HomeButton.onClick.RemoveListener(Home);
        }
    }
}