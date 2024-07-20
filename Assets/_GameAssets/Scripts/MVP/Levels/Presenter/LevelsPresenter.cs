using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Factories;
using TapAndRun.Factories.Levels;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.View;
using Unity.Mathematics;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Presenter
{
    public class LevelsPresenter
    {
        
        private CancellationTokenSource _cts;

        private const int MaxLevelViews = 3;
        
        private readonly LevelFactory _levelFactory;
        private readonly Queue<LevelView> _levels;
        private readonly LevelsModel _model;

        public LevelsPresenter(LevelsModel model, LevelFactory levelFactory)
        {
            _model = model;
            _levelFactory = levelFactory;
        }

        public void Initialize()
        {
            _cts = new CancellationTokenSource();

            _model.OnLevelChanged += BuildLevel;
        }

        private void BuildLevel()
        {
            BuildLevelAsync(_cts.Token).Forget();

            async UniTask BuildLevelAsync(CancellationToken token)
            {
                await ClearLevels(token);

                var levelView = await _levelFactory.CreateLevelViewAsync(_model.CurrentLevelId, Vector2.zero, Quaternion.identity, token);

                _levels.Enqueue(levelView);
                _model.SetCommands(levelView.Interactions);

                await BuildNextLevel(token);

                _model.IsLevelBuild = true;
            }
        }

        private async UniTask BuildNextLevel(CancellationToken token)
        {
            if (_levels.Count == MaxLevelViews)
            {
                ClearLastLevel();
            }

            var nextLevelId = _model.CurrentLevelId + 1;

            var newLevel = await _levelFactory.CreateLevelViewAsync(nextLevelId, Vector2.zero, Quaternion.identity, token);
            _levels.Enqueue(newLevel);
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
    }
}