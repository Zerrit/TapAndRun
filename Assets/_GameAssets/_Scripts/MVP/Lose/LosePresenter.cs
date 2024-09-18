using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.MVP.CharacterCamera.Model;
using TapAndRun.MVP.Lose.Model;
using TapAndRun.MVP.Lose.View;
using TapAndRun.Services.Audio;

namespace TapAndRun.MVP.Lose
{
    public class LosePresenter : IInitializableAsync, IDecomposable
    {
        private readonly ISelfLoseModel _model;
        private readonly ICameraZoom _cameraZoom;
        private readonly IAudioService _audioService;
        private readonly LoseView _view;

        private CancellationTokenSource _cts;

        public LosePresenter(ISelfLoseModel model, ICameraZoom cameraZoom, IAudioService audioService, LoseView view)
        {
            _model = model;
            _cameraZoom = cameraZoom;
            _audioService = audioService;
            _view = view;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();

            _model.IsDisplaying.OnChanged += UpdateDisplaying;
            _view.RestartButton.onClick.AddListener(Restart);
            _view.HomeButton.onClick.AddListener(Home);
            _view.BackgroundButton.onClick.AddListener(TurnFreeView);

            return UniTask.CompletedTask;
        }

        private void UpdateDisplaying(bool status)
        {
            if (status)
            {
                _model.ProcessLose();
                _view.Show();
                _cameraZoom.SetLoseZoomAsync().Forget();
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
            _audioService.PlaySound("Click");
            _model.HomeTrigger.Trigger();
        }

        private void TurnFreeView()
        {
            TurnFreeViewAsync(_cts.Token).Forget();

            async UniTaskVoid TurnFreeViewAsync(CancellationToken token)
            {
                _cameraZoom.SetFreeViewZoomAsync().Forget();

                await _view.HideAsync(token);

                _view.FreeViewButton.gameObject.SetActive(true);

                await _view.FreeViewButton.OnClickAsync(token);
            
                _cameraZoom.SetLoseZoomAsync().Forget();
                _view.FreeViewButton.gameObject.SetActive(false);
                _view.Show();
            }
        }

        public void Decompose()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _model.IsDisplaying.OnChanged -= UpdateDisplaying;
            _view.RestartButton.onClick.RemoveListener(Restart);
            _view.HomeButton.onClick.RemoveListener(Home);
            _view.BackgroundButton.onClick.RemoveListener(TurnFreeView);
        }
    }
}