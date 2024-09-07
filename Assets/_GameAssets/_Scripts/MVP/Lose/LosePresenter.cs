using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Lose.Model;
using TapAndRun.MVP.Lose.View;

namespace TapAndRun.MVP.Lose
{
    public class LosePresenter : IInitializableAsync, IDecomposable
    {
        private readonly ISelfLoseModel _model;
        private readonly LoseView _view;

        public LosePresenter(ISelfLoseModel model, LoseView view)
        {
            _model = model;
            _view = view;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            _model.IsDisplaying.OnChanged += UpdateDisplaying;
            _view.RestartButton.onClick.AddListener(_model.RestartTrigger.Trigger);
            _view.HomeButton.onClick.AddListener(_model.HomeTrigger.Trigger);

            return UniTask.CompletedTask;
        }

        private void UpdateDisplaying(bool status)
        {
            if (status)
            {
                _view.Show();
            }
            else
            {
                _view.Hide();
            }
        }

        public void Decompose()
        {
            _model.IsDisplaying.OnChanged -= UpdateDisplaying;
            _view.RestartButton.onClick.RemoveListener(_model.RestartTrigger.Trigger);
            _view.HomeButton.onClick.RemoveListener(_model.HomeTrigger.Trigger);
        }
    }
}