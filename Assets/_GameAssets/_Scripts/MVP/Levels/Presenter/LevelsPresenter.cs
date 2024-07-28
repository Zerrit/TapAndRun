using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Factories.Levels;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.View;
using TapAndRun.MVP.Screens.LevelScreen;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TapAndRun.MVP.Levels.Presenter
{
    public class LevelsPresenter : IDisposable
    {
        private LevelView _actualLevel;
        private LevelView _lastLevel;

        private CancellationTokenSource _cts;

        private const int MaxLevelViews = 3;

        private readonly Queue<LevelView> _levels = new();
        private readonly LevelScreenView _levelScreen;
        private readonly ILevelsSelfModel _selfModel;
        private readonly ILevelFactory _levelFactory;
        private readonly ICharacterModel _characterModel;

        public LevelsPresenter(ILevelsSelfModel selfModel, ILevelFactory levelFactory, ICharacterModel characterModel, LevelScreenView levelScreen)
        {
            _selfModel = selfModel;
            _levelFactory = levelFactory;
            _characterModel = characterModel;
            _levelScreen = levelScreen;
        }

        public void Initialize()
        {
            _cts = new CancellationTokenSource();

            _selfModel.IsScreenDisplaying.OnChanged += ChangeScreenDisplaying;
            _selfModel.OnLevelChanged += BuildLevel;
            _selfModel.OnLevelCompleted += BuildNextLevel;
            _levelScreen.OnClicked += _selfModel.ApplyClick;
        }

        private void BuildLevel()
        {
            BuildLevelAsync(_cts.Token).Forget();

            async UniTask BuildLevelAsync(CancellationToken token)
            {
                await ClearLevels(token);

                _lastLevel = await _levelFactory.CreateLevelViewAsync(_selfModel.CurrentLevelId, Vector2.zero, Quaternion.identity, token);

                _actualLevel = _lastLevel;
                _levels.Enqueue(_lastLevel);
                _selfModel.SetCommands(_lastLevel.Interactions);

                _characterModel.Replace(_lastLevel.StartSegment.SegmentCenter.position);

                BuildNextLevel();

                _selfModel.IsLevelBuild = true;
            }
        }

        private void BuildNextLevel()
        {
            BuildNextLevelAsync(_cts.Token).Forget();
            
            async UniTask BuildNextLevelAsync(CancellationToken token)
            {
                if (_levels.Count == MaxLevelViews)
                {
                    ClearLastLevel();
                }

                var nextLevelId = _selfModel.CurrentLevelId + 1;

                _lastLevel = await _levelFactory.CreateLevelViewAsync(nextLevelId, _lastLevel.FinishSegment.SnapPoint.position, Quaternion.identity, token);

                _levels.Enqueue(_lastLevel);
            }
        }

        private void ClearLastLevel()
        {
            Object.Destroy(_levels.Dequeue().gameObject);
            
            //TODO Удалить ссылку на операцию в фабрике.
        }
        
        private async UniTask ClearLevels(CancellationToken token)
        {
            foreach (var level in _levels)
            {
                Object.Destroy(level.gameObject);
            }

            _levels.Clear();
            _levelFactory.Dispose();
        }

        private void ChangeScreenDisplaying(bool status)
        {
            if (status)
            {
                _levelScreen.Show();
            }
            else
            {
                _levelScreen.Hide();
            }
        }
        
        
        public void Dispose()
        {
            _selfModel.OnLevelChanged -= BuildLevel;
            _levelScreen.OnClicked -= _selfModel.ApplyClick;
            
            _cts?.Dispose();
        }
    }
}