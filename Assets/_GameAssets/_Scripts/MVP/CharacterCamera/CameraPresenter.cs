﻿using System.Threading;
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
            _model.Position.OnChanged += _view.UpdatePosition;
            _model.Rotation.OnChanged += _view.UpdateRotation;
            _model.Height.OnChanged += _view.UpdateHeight;
            
            return UniTask.CompletedTask;
        }

        public void Decompose()
        {
            _model.Position.OnChanged -= _view.UpdatePosition;
            _model.Rotation.OnChanged -= _view.UpdateRotation;
            _model.Height.OnChanged -= _view.UpdateHeight;
        }
    }
}