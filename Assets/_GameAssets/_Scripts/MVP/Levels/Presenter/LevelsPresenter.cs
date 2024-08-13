using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.CameraLogic;
using TapAndRun.Factories.Levels;
using TapAndRun.MVP.Character.Commands;
using TapAndRun.MVP.Character.View;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.View;
using TapAndRun.MVP.Screens.Level;
using TapAndRun.MVP.Screens.Lose;
using TapAndRun.MVP.Screens.Wallet;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Presenter
{
    public class LevelsPresenter : IDisposable
    {
        private LevelView _oldLevel;
        private LevelView _currentLevel;
        private LevelView _nextLevel;

        private CancellationTokenSource _cts;

        private readonly List<ICommand> _characterCommands = new ();

        private readonly CharacterView _character;
        private readonly CharacterCamera _camera;
        private readonly WalletView _walletView;
        private readonly LevelScreenView _levelScreen;
        private readonly LoseScreenView _loseScreen;

        private readonly ILevelsSelfModel _selfModel;
        private readonly ILevelFactory _levelFactory;

        public LevelsPresenter(ILevelsSelfModel selfModel, ILevelFactory levelFactory, 
            CharacterView character, CharacterCamera camera, WalletView walletView, LevelScreenView levelScreen, LoseScreenView loseScreen)
        {
            _selfModel = selfModel;
            _levelFactory = levelFactory;
            _character = character;
            _camera = camera;
            _walletView = walletView;
            _levelScreen = levelScreen;
            _loseScreen = loseScreen;
        }

        public async UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();
            _camera.Initialize(_character.CharacterTransform);
            
            await BuildLevelAsync(token);

            _selfModel.AvailableCrystals.SubscribeAndUpdate(_walletView.UpdateAvailableCrystals);
            _selfModel.CrystalsByLevel.SubscribeAndUpdate(_walletView.UpdateCrystalsByLevel);
            _loseScreen.RestartButton.onClick.AddListener(RestartGameplay);

            _selfModel.OnLevelStarted += StartGameplay;
            _selfModel.OnLevelCompleted += UpdateLevelViews;

            _character.OnFalling += ShowLose;
            _levelScreen.OnClicked += ProcessClick;
        }

        private void StartGameplay()
        {
            _levelScreen.Show();
            //TODO Возможно сделать задержку для анимаций перед запуском персонажа

            _character.StartMove();
        }

        private async void RestartGameplay()
        {
            _loseScreen.Hide();
            
            await BuildLevelAsync(_cts.Token);
            
            StartGameplay();
            //_levelScreen.Show();
            //_character.StartMove();
        }
        
        private void ShowLose()
        {
            ShowLoseAsync(_cts.Token).Forget();
            
            async UniTaskVoid ShowLoseAsync(CancellationToken token)
            {
                _selfModel.LoseLevel();
                _levelScreen.Hide();

                await _camera.FlyUpAsync(_cts.Token);

                _loseScreen.Show();
            }
        }

        private async UniTask BuildLevelAsync(CancellationToken token)
        {
            await ClearLevels(token);

            _nextLevel = await _levelFactory.CreateLevelViewAsync(_selfModel.CurrentLevelId, Vector2.zero, Quaternion.identity, token);

            _nextLevel.Configure(_selfModel.CurrentLevelId);

            ActivateNextLevel();
            BuildNextLevel(); 

            _character.MoveTo(_currentLevel.StartSegment.SegmentCenter.position);
            _camera.ResetRotation();
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

        private void UpdateLevelViews()
        {
            ClearOldLevel();

            DeactivateLevel();
            ActivateNextLevel();

            BuildNextLevel();

            _character.CenteringAsync(_currentLevel.StartSegment.SegmentCenter.position).Forget();
        }

        private void ActivateNextLevel()
        {
            _currentLevel = _nextLevel;
            
            UpdateCommands();
            _character.ChangeSpeed(_selfModel.CurrentDifficulty);
            _camera.ChangeDifficulty(_selfModel.CurrentDifficulty, _cts.Token);
            //TODO Включение учета интерактивных стрелок

            foreach (var crystal in _currentLevel.Crystals)
            {
                crystal.OnTaken += _selfModel.IncreaseCrystals;
            }

            _currentLevel.FinishSegment.OnPlayerEntered += _selfModel.CompleteLevel;
        }

        private void DeactivateLevel()
        {
            if (!_currentLevel)
            {
                return;
            }

            _oldLevel = _currentLevel;

            foreach (var crystal in _currentLevel.Crystals)
            {
                crystal.OnTaken -= _selfModel.IncreaseCrystals;
            }
 
            _currentLevel.OnFinishReached -= _selfModel.CompleteLevel;
        }

        /// <summary>
        /// Обрабатывает клик игрока.
        /// </summary>
        private void ProcessClick()
        {
            if (_selfModel.CurrentInteractionIndex >= _selfModel.InteractionCount)
            {
                return;
            }
            
            _characterCommands[_selfModel.CurrentInteractionIndex].Execute();
            _selfModel.CurrentInteractionIndex++;
        }

        /// <summary>
        /// Обновляет список команд для персонажа, согласно активному уровню.
        /// </summary>
        private void UpdateCommands()
        {
            _characterCommands.Clear();

            _selfModel.InteractionCount = _currentLevel.InteractionPoints.Count;
            _selfModel.CurrentInteractionIndex = 0;

            foreach (var interact in _currentLevel.InteractionPoints)
            {
                switch (interact.CommandType)
                {
                    case InteractType.Jump:
                    {
                        _characterCommands.Add(new JumpCommand(_character, _camera));
                        break;
                    }
                    case InteractType.TurnLeft:
                    {
                        _characterCommands.Add(new TurnLeftCommand(_character, _camera));
                        break;
                    }
                    case InteractType.TurnRight:
                    {
                        _characterCommands.Add(new TurnRightCommand(_character, _camera));
                        break;
                    }
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
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
            /*_selfModel.OnLevelChanged -= BuildLevel;
            _selfModel.OnLevelStarted -= StartGameplay;
            _selfModel.OnLevelCompleted -= UpdateLevelViews;
            _levelScreen.OnClicked -= ProcessClick;*/

            _cts?.Dispose();
        }
    }
}