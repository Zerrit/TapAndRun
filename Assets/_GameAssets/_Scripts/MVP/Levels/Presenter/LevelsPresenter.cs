using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.CameraLogic;
using TapAndRun.Factories.Levels;
using TapAndRun.MVP.Character.Commands;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Character.View;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.View;
using TapAndRun.MVP.Screens.Level;
using TapAndRun.MVP.Screens.Lose;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Presenter
{
    public class LevelsPresenter : IDisposable
    {
        private LevelView _oldLevel;
        private LevelView _currentLevel;
        private LevelView _nextLevel;

        private CancellationTokenSource _cts;

        //private ICommand _activeCommand;
        private readonly List<ICommand> _characterCommands = new ();

        private readonly CharacterView _character;
        private readonly CharacterCamera _camera;
        private readonly LevelScreenView _levelScreen;
        private readonly LoseScreenView _loseScreen;

        private readonly ILevelsSelfModel _selfModel;
        private readonly ILevelFactory _levelFactory;

        public LevelsPresenter(ILevelsSelfModel selfModel, ILevelFactory levelFactory, 
            CharacterView character, CharacterCamera camera, LevelScreenView levelScreen, LoseScreenView loseScreen)
        {
            _selfModel = selfModel;
            _levelFactory = levelFactory;
            _character = character;
            _camera = camera;
            _levelScreen = levelScreen;
            _loseScreen = loseScreen;
        }

        public void Initialize()
        {
            _cts = new CancellationTokenSource();
            _camera.SetCharacter(_character.CharacterTransform);

            _selfModel.OnLevelChanged += BuildLevel;
            _selfModel.OnLevelStarted += StartGameplay;
            _selfModel.OnLevelCompleted += UpdateLevelViews;

            _levelScreen.OnClicked += ProcessClick;
        }

        private void StartGameplay()
        {
            _levelScreen.Show();
            //TODO Возможно сделать задержку для анимаций перед запуском персонажа

            _character.StartMove(new Vector2(0, 1));
        }

        private void BuildLevel()
        {
            BuildLevelAsync(_cts.Token).Forget();

            async UniTask BuildLevelAsync(CancellationToken token)
            {
                await ClearLevels(token);

                _nextLevel = await _levelFactory.CreateLevelViewAsync(_selfModel.CurrentLevelId, Vector2.zero, Quaternion.identity, token);

                _nextLevel.Configure(-_selfModel.CurrentLevelId);

                ActivateNextLevel();
                BuildNextLevel(); 

                MoveCharacterTo(_currentLevel.StartSegment.SegmentCenter.position);

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

                _nextLevel.Configure(nextLevelId);
            }
        }

        private void ActivateNextLevel()
        {
            if (_currentLevel)
            {
                _oldLevel = _currentLevel;
                _currentLevel.OnFinishReached -= _selfModel.CompleteLevel;
            }
            
            _currentLevel = _nextLevel;
            UpdateCommands();
            //TODO Включение учета интерактивных стрелок
            
            _currentLevel.OnFinishReached += _selfModel.CompleteLevel;
        }

        private void UpdateLevelViews()
        {
           // _characterModel.
            ClearOldLevel();

            ActivateNextLevel();
            BuildNextLevel();
        }

        private void ProcessClick()
        {
            if (_selfModel.CurrentInteractionIndex >= _selfModel.InteractionCount)
            {
                return;
            }
            
            _characterCommands[_selfModel.CurrentInteractionIndex].Execute();
        }
        
        /*public void ExecuteCommand(int interactionIndex)
        {
            _activeCommand = _characterCommands[interactionIndex];
            _activeCommand.Execute();
        }*/

        /// <summary>
        /// Обновляет список команд для персонажа, согласно активному уровню.
        /// </summary>
        private void UpdateCommands()
        {
            _characterCommands.Clear();

            _selfModel.InteractionCount = _currentLevel.Interactions.Count;
            _selfModel.CurrentInteractionIndex = 0;

            foreach (var interact in _currentLevel.Interactions)
            {
                switch (interact)
                {
                    case InteractType.Jump:
                    {
                        _characterCommands.Add(new JumpCommand(_character));
                        break;
                    }
                    case InteractType.TurnLeft:
                    {
                        _characterCommands.Add(new TurnLeftCommand(_character));
                        break;
                    }
                    case InteractType.TurnRight:
                    {
                        _characterCommands.Add(new TurnRightCommand(_character));
                        break;
                    }
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        private void MoveCharacterTo(Vector2 position)
        {
            _character.CharacterTransform.position = position;
        }
        
        private void ClearOldLevel()
        {
            if (_oldLevel)
            {
                _oldLevel.Destroy();
            }

            //TODO Удалить ссылку на операцию в фабрике.
        }
        
        private async UniTask ClearLevels(CancellationToken token)
        {
            if (_oldLevel)
            {
                _oldLevel.Destroy();
            }

            if (_currentLevel)
            {
                _currentLevel.Destroy();
            }

            if (_nextLevel)
            {
                _nextLevel.Destroy();
            }

            _levelFactory.Dispose();
        }

        public void Dispose()
        {
            _selfModel.OnLevelChanged -= BuildLevel;
            _selfModel.OnLevelStarted -= StartGameplay;
            _selfModel.OnLevelCompleted -= UpdateLevelViews;
            _levelScreen.OnClicked -= ProcessClick;

            _cts?.Dispose();
        }
    }
}