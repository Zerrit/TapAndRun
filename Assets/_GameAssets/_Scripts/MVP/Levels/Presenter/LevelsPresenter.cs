using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Factories.Levels;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.CharacterCamera;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.Views;
using TapAndRun.MVP.Wallet.Model;
using TapAndRun.Services.Audio;
using TapAndRun.TapSystem;
using UnityEngine;

namespace TapAndRun.MVP.Levels.Presenter
{
    public class LevelsPresenter : IInitializableAsync, IDisposable
    {
        private LevelView _oldLevel;
        private LevelView _currentLevel;
        private LevelView _nextLevel;

        private CancellationTokenSource _cts;

        private TapCommandHandler _commandHandler;

        private readonly GameplayScreenView _gameplayScreen;

        private readonly ICharacterModel _characterModel;
        private readonly ICameraModel _cameraModel;
        private readonly IWalletModel _walletModel;
        private readonly ISelfLevelsModel _model;
        private readonly ILevelFactory _levelFactory;
        private readonly IAudioService _audioService;

        public LevelsPresenter(ISelfLevelsModel model, ILevelFactory levelFactory, IAudioService audioService,
            ICharacterModel characterModel, ICameraModel cameraModel, IWalletModel walletModel, 
            GameplayScreenView gameplayScreen)
        {
            _model = model;
            _levelFactory = levelFactory;
            _audioService = audioService;
            _characterModel = characterModel;
            _cameraModel = cameraModel;
            _walletModel = walletModel;
            _gameplayScreen = gameplayScreen;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();
            _commandHandler = new TapCommandHandler(_characterModel, _cameraModel);
            _model.LevelCount = _levelFactory.GetLevelCount();

            _model.OnLevelChanged += BuildLevel;
            _model.OnLevelReseted += ResetLevel;
            _model.OnLevelStarted += StartGameplay;

            _characterModel.IsFall.OnChangedToTrue += ProcessLevelLose;
            _gameplayScreen.OnClicked += ProcessClick;

            return UniTask.CompletedTask;
        }

        private void StartGameplay()
        {
            _gameplayScreen.Show();
            _characterModel.StartMove();
        }

        private void BuildLevel()
        {
            BuildLevelAsync(_cts.Token).Forget();
            
            async UniTask BuildLevelAsync(CancellationToken token)
            {
                ClearLevels();

                _nextLevel = await _levelFactory.CreateLevelViewAsync(_model.CurrentLevelId, Vector2.zero, Quaternion.identity, token);

                _nextLevel.Configure(_model.CurrentLevelId);

                ActivateNextLevel();
                BuildNextLevel();

                _characterModel.MoveTo(_currentLevel.StartSegment.SegmentCenter.position);
                _cameraModel.SetRotation();
            }
        }

        private void BuildNextLevel()
        {
            if (!_model.CheckLevelExist(_model.CurrentLevelId + 1))
            {
                return;
            }

            BuildNextLevelAsync(_cts.Token).Forget();
            
            async UniTask BuildNextLevelAsync(CancellationToken token)
            {
                var nextLevelId = _model.CurrentLevelId + 1;

                _nextLevel = await _levelFactory.CreateLevelViewAsync(nextLevelId, _currentLevel.FinishSegment.SnapPoint.position, _currentLevel.FinishSegment.SnapPoint.rotation, token);

                _nextLevel.Configure(nextLevelId);
            }
        }

        private void ResetLevel()
        {
            ClearOldLevel();

            _currentLevel.Refresh();
            _currentLevel.ActivateArrow();
            _commandHandler.ResetCommandIndex();

            var startRotation = _currentLevel.transform.eulerAngles.z;
            _characterModel.MoveTo(_currentLevel.StartSegment.SegmentCenter.position, startRotation);
            _cameraModel.SetRotation(startRotation);

            UpdateDifficulty();
        }
        
        /// <summary>
        /// Передает уровень сложности в персонажа и камеру.
        /// </summary>
        private void UpdateDifficulty()
        {
            _characterModel.ChangeSpeed(_model.CurrentDifficulty);
            _cameraModel.ChangeDifficulty(_model.CurrentDifficulty);
        }

        /// <summary>
        /// Обрабатывает клик игрока.
        /// </summary>

        private void DeactivateLevel()
        {
            if (!_currentLevel)
            {
                return;
            }

            _oldLevel = _currentLevel;

            foreach (var crystal in _currentLevel.Crystals)
            {
                crystal.OnTaken -= TakeCrystal;
            }
 
            _currentLevel.OnFinishReached -= ProcessLevelComplete;
        }
        
        private void ActivateNextLevel()
        {
            _currentLevel = _nextLevel;

            _commandHandler.GenerateCommand(_currentLevel.InteractionPoints);
            UpdateDifficulty();

            _nextLevel.ActivateArrow();

            foreach (var crystal in _currentLevel.Crystals)
            {
                crystal.OnTaken += TakeCrystal;
            }

            _currentLevel.FinishSegment.OnPlayerEntered += ProcessLevelComplete;
        }
        
        private void ProcessClick()
        {
            if (!_commandHandler.CheckAvailability())
            {
                return;
            }

            _commandHandler.ExecuteCommand().Forget();
            _currentLevel.SwitchToNextArrow(_commandHandler.CurrentInteractionIndex);
        }

        private void ProcessLevelLose()  //TODO Подумать ещё над неймингом
        {
            _model.LoseLevel();
            _walletModel.ResetCrystalsByLevel();

            _gameplayScreen.Hide();  //TODO Донастроить скрипт окна геймплейного
            _cameraModel.FlyUpAsync().Forget(); //TODO Возвомжно изменить логику
        }

        private void ProcessLevelComplete() //TODO Подумать ещё над неймингом
        {
            _model.CompleteLevel();
            _walletModel.GainCrystalsByLevel();

            ClearOldLevel();

            DeactivateLevel();
            ActivateNextLevel();

            BuildNextLevel();

            _characterModel.CenteringAsync(_currentLevel.StartSegment.SegmentCenter.position).Forget();
            _audioService.PlaySound("Finish");
        }

        private void TakeCrystal()
        {
            _audioService.PlaySound("TakeCrystal");
            _walletModel.IncreaseCrystalsByLevel();
        }
        
        private void ClearOldLevel()
        {
            if (_oldLevel)
            {
                _oldLevel.Destroy();
                _levelFactory.DisposeOldLevel();
            }

            //TODO Удалить ссылку на операцию в фабрике.
        }
        
        private void ClearLevels()
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
            _model.OnLevelChanged -= BuildLevel;
            _model.OnLevelReseted -= ResetLevel;
            _model.OnLevelStarted -= StartGameplay;

            _characterModel.IsFall.OnChangedToTrue -= ProcessLevelLose;
            _gameplayScreen.OnClicked -= ProcessClick;

            _cts.Cancel();
            _cts?.Dispose();
        }
    }
}