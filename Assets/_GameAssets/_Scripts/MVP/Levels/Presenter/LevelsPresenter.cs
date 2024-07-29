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
        private int _maxLevelViews;

        private LevelView _oldLevel;
        private LevelView _currentLevel;
        private LevelView _nextLevel;

        private CancellationTokenSource _cts;

        private readonly List<LevelView> _levels = new();
        private readonly LevelScreenView _levelScreen;
        private readonly ILevelsSelfModel _selfModel;
        private readonly ILevelFactory _levelFactory;
        private readonly ICharacterModel _characterModel;

        public LevelsPresenter(ILevelsSelfModel selfModel, ILevelFactory levelFactory, 
            ICharacterModel characterModel, LevelScreenView levelScreen)
        {
            _selfModel = selfModel;
            _levelFactory = levelFactory;
            _characterModel = characterModel;
            _levelScreen = levelScreen;
        }

        public void Initialize()
        {
            _cts = new CancellationTokenSource();

            _maxLevelViews = 3;

            _selfModel.OnLevelChanged += BuildLevel;
            _selfModel.OnLevelStarted += ChangeScreenDisplaying;
            _selfModel.OnLevelCompleted += EnableNexLevel;

            _levelScreen.OnClicked += _selfModel.ApplyClick;
        }

        private void ChangeScreenDisplaying() //TODO Изменить логику названия
        {
            _levelScreen.Show();
        }

        private void BuildLevel()
        {
            BuildLevelAsync(_cts.Token).Forget();

            async UniTask BuildLevelAsync(CancellationToken token)
            {
                await ClearLevels(token);

                _currentLevel = await _levelFactory.CreateLevelViewAsync(_selfModel.CurrentLevelId, Vector2.zero, Quaternion.identity, token);
                
                _currentLevel.Configure(-_selfModel.CurrentLevelId);
                _levels.Add(_currentLevel);
                _characterModel.Replace(_currentLevel.StartSegment.SegmentCenter.position);

                BuildNextLevel(); 
                ActivateLevel(_currentLevel); //TODO Подумать об активации сразу

                _selfModel.IsLevelBuild = true;
            }
        }

        private void BuildNextLevel()
        {
            BuildNextLevelAsync(_cts.Token).Forget();
            
            async UniTask BuildNextLevelAsync(CancellationToken token)
            {
                var nextLevelId = _selfModel.CurrentLevelId + 1;

                _nextLevel = await _levelFactory.CreateLevelViewAsync(nextLevelId, _currentLevel.FinishSegment.SnapPoint.position, _currentLevel.FinishSegment.SnapPoint.rotation, token);

                _currentLevel.Configure(nextLevelId);
                _levels.Add(_nextLevel);
            }
        }

        private void ActivateLevel(LevelView level)
        {
            if (_oldLevel)
            {
                _oldLevel.OnFinishReached -= _selfModel.CompleteLevel;
            }

            _oldLevel = _currentLevel;
            _currentLevel = level;
            _selfModel.SetCommands(_currentLevel.Interactions);
            //TODO Включение учета интерактивных стрелок
            
            _currentLevel.OnFinishReached += _selfModel.CompleteLevel;
        }

        private void EnableNexLevel()
        {
            if (_levels.Count == _maxLevelViews)
            {
                ClearOldLevel();
            }

            ActivateLevel(_nextLevel);
            BuildNextLevel();
        }
        
        private void ClearOldLevel()
        {
            Object.Destroy(_levels[0].gameObject);
            
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

        public void Dispose()
        {
            _selfModel.OnLevelChanged -= BuildLevel;
            _levelScreen.OnClicked -= _selfModel.ApplyClick;
            
            _cts?.Dispose();
        }
    }
}