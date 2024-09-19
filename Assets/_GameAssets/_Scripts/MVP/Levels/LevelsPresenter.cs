using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Factories.Levels;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.CharacterCamera.Model;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.Views;
using TapAndRun.MVP.Levels.Views.Tutorial;
using TapAndRun.MVP.Wallet.Model;
using TapAndRun.PrallaxBackground;
using TapAndRun.Services.Audio;
using TapAndRun.TapSystem;
using UnityEngine;

namespace TapAndRun.MVP.Levels
{
    public class LevelsPresenter : IInitializableAsync, IDecomposable
    {
        private LevelView _oldLevel;
        private LevelView _currentLevel;
        private LevelView _nextLevel;

        private CancellationTokenSource _cts;
        private TapCommandHandler _commandHandler;

        private readonly IParallaxView _parallaxView;
        private readonly GameplayScreenView _gameplayScreen;

        private readonly ICharacterModel _characterModel;
        private readonly ICameraModel _cameraModel;
        private readonly ISelfLevelsModel _model;
        private readonly ILevelFactory _levelFactory;
        private readonly IAudioService _audioService;

        public LevelsPresenter(ISelfLevelsModel model, ILevelFactory levelFactory, IAudioService audioService,
            ICharacterModel characterModel, ICameraModel cameraModel, IParallaxView parallaxView, GameplayScreenView gameplayScreen)
        {
            _model = model;
            _levelFactory = levelFactory;
            _audioService = audioService;
            _characterModel = characterModel;
            _cameraModel = cameraModel;
            _parallaxView = parallaxView;
            _gameplayScreen = gameplayScreen;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();
            _commandHandler = new TapCommandHandler(_characterModel, _cameraModel);
            _model.LevelCount = _levelFactory.GetLevelCount();

            _model.IsDisplaying.Subscribe(BuildLevel, HideLevels);
            _model.IsTutorialDisplaying.Subscribe(_gameplayScreen.TutorialView.Show, _gameplayScreen.TutorialView.Hide);

            _model.StartupTrigger.Subscribe(StartGameplay);
            _model.ResetLevelTrigger.Subscribe(ResetLevel);
            _model.CurrentDifficulty.Subscribe(_gameplayScreen.UpdateSpeedText);
            _characterModel.IsFall.Subscribe(ProcessLevelLose, true);

            _model.OnLevelChanged += BuildLevel;
            _gameplayScreen.TapButton.onClick.AddListener(ProcessClick);
            
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
                //_model.ResetDifficulty();

                _nextLevel = await _levelFactory.CreateLevelViewAsync(_model.CurrentLevelId, Vector2.zero, Quaternion.identity, token);

                _nextLevel.Configure(_model.CurrentLevelId);
                _characterModel.MoveTo(_nextLevel.StartSegment.SegmentCenter.position);
                _cameraModel.SetRotation();

                ActivateNextLevel();
                BuildNextLevel();
            }
        }

        private void BuildNextLevel()
        {
            BuildNextLevelAsync(_cts.Token).Forget();
            
            async UniTask BuildNextLevelAsync(CancellationToken token)
            {
                var nextLevelId = _model.NextLevelId;

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

            _commandHandler.GenerateCommands(_currentLevel.InteractionPoints);
            UpdateDifficulty();

            _gameplayScreen.UpdateLevelText(_model.CurrentLevelId);
            _currentLevel.ActivateArrow();

            foreach (var crystal in _currentLevel.Crystals)
            {
                crystal.OnTaken += TakeCrystal;
            }

            _currentLevel.FinishSegment.OnPlayerEntered += ProcessLevelComplete;

            CheckLevelForTutorial();
        }

        private void UpdateDifficulty()
        {
            _model.CurrentDifficulty.Value = Mathf.Max(_model.CurrentDifficulty.Value, _currentLevel.SpeedDifficulty);

            _characterModel.ChangeSpeed(_model.CurrentDifficulty.Value);
            _cameraModel.ChangeMode(_model.CurrentDifficulty.Value, _currentLevel.CameraMode);
        }

        private void ProcessClick()
        {
            _model.OnTapTrigger.Trigger();

            if (!_commandHandler.CheckAvailability())
            {
                return;
            }

            _commandHandler.ExecuteCommand().Forget();
            _currentLevel.SwitchToNextArrow(_commandHandler.CurrentInteractionIndex);
        }

        private void ProcessLevelLose()
        {
            _model.LoseLevel();
            _audioService.CallVibration();
            _gameplayScreen.Hide();
        }

        private void ProcessLevelComplete()
        {
            _model.CompleteLevel();

            ClearOldLevel();

            DeactivateLevel();
            ActivateNextLevel();

            BuildNextLevel();

            _characterModel.CenteringAsync(_currentLevel.StartSegment.SegmentCenter.position).Forget();
            _parallaxView.ChangeStyle();
            _audioService.PlaySound("Finish");
        }

        private void TakeCrystal()
        {
            //TODO Может быть проигран SFX.
            _model.AddCrystalByRun();
        }

        private void CheckLevelForTutorial()
        {
            if (_model.IsTutorialComplete || _model.CurrentLevelId != 0)
            {
                return;
            }

            var tutorLevel = _currentLevel as TutorialLevelView;
            if (tutorLevel)
            {
                _model.IsTutorialLevel = true;

                tutorLevel.OnPlayerEnterToTutorial += _model.OnEnterToInteractPointTrigger.Trigger;
            }
        }

        private void HideLevels() //TODO нейминг
        {
            _characterModel.IsActive.Value = false;
            DeactivateLevel();
            ClearLevels();
        }
        
        private void ClearOldLevel()
        {
            if (_oldLevel)
            {
                _oldLevel.Destroy();
                _levelFactory.DisposeOldLevel();
            }
        }

        private void ClearLevels()
        {
            /*if (_oldLevel)
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
            }*/

            _levelFactory.Decompose();
        }

        public void Decompose()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _model.IsDisplaying.Unsubscribe(BuildLevel, HideLevels);
            _model.IsTutorialDisplaying.Unsubscribe(_gameplayScreen.TutorialView.Show, _gameplayScreen.TutorialView.Hide);

            _model.StartupTrigger.Unsubscribe(StartGameplay);
            _model.ResetLevelTrigger.Unsubscribe(ResetLevel);
            _model.CurrentDifficulty.Unsubscribe(_gameplayScreen.UpdateSpeedText);
            _characterModel.IsFall.Unsubscribe(ProcessLevelLose, true);

            _model.OnLevelChanged -= BuildLevel;
            _gameplayScreen.TapButton.onClick.RemoveListener(ProcessClick);
        }
    }
}