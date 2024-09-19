using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.MVP.CharacterCamera.Model;

namespace TapAndRun.MVP.CharacterCamera
{
    public class CameraPresenter : IInitializableAsync, IDecomposable
    {
        private readonly ISelfCameraModel _model;
        private readonly CameraView _view;

        public CameraPresenter(ISelfCameraModel model, CameraView view)
        {
            _model = model;
            _view = view;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            _model.Position.Subscribe(_view.UpdatePosition);
            _model.Rotation.Subscribe(_view.UpdateRotation);
            _model.Height.Subscribe(_view.UpdateHeight);

            return UniTask.CompletedTask;
        }

        public void Decompose()
        {
            _model.Position.Unsubscribe(_view.UpdatePosition);
            _model.Rotation.Unsubscribe(_view.UpdateRotation);
            _model.Height.Unsubscribe(_view.UpdateHeight);
        }
    }
}