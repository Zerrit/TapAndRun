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
using TapAndRun.PrallaxBackground.OffsetBackground;
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

        private readonly IParallaxView _view;
        private readonly GameplayScreenView _gameplayScreen;

        private readonly ICharacterModel _characterModel;
        private readonly ICameraModel _cameraModel;
        private readonly IWalletTutorial _walletTutorial;
        private readonly ISelfLevelsModel _model;
        private readonly ILevelFactory _levelFactory;
        private readonly IAudioService _audioService;

        public LevelsPresenter(ISelfLevelsModel model, ILevelFactory levelFactory, IAudioService audioService,
            ICharacterModel characterModel, ICameraModel cameraModel, IWalletTutorial walletTutorial,
            IParallaxView view, GameplayScreenView gameplayScreen)
        {
            _model = model;
            _levelFactory = levelFactory;
            _audioService = audioService;
            _characterModel = characterModel;
            _cameraModel = cameraModel;
            _walletTutorial = walletTutorial;
            _view = view;
            _gameplayScreen = gameplayScreen;
        }

        public UniTask InitializeAsync(CancellationToken token)
        {
            _cts = new CancellationTokenSource();
            _commandHandler = new TapCommandHandler(_characterModel, _cameraModel);
            _model.LevelCount = _levelFactory.GetLevelCount();

            _model.IsDisplaying.Subscribe(BuildLevel, true);
            _model.IsDisplaying.Subscribe(HideLevels, false);
            
            _model.StartupTrigger.Subscribe(StartGameplay);
            _model.ResetLevelTrigger.Subscribe(ResetLevel);

            _model.OnLevelChanged += BuildLevel;

            _characterModel.IsFall.OnChangedToTrue += ProcessLevelLose;
            _gameplayScreen.TapButton.onClick.AddListener(ProcessClick);
            
            return UniTask.CompletedTask;
        }

        private void StartGameplay()
        {
            CheckForTutorials();
            
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
                _characterModel.MoveTo(_nextLevel.StartSegment.SegmentCenter.position);
                _cameraModel.SetRotation();
                
                ActivateNextLevel();
                BuildNextLevel();
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

            _currentLevel.ActivateArrow();

            foreach (var crystal in _currentLevel.Crystals)
            {
                crystal.OnTaken += TakeCrystal;
            }

            _currentLevel.FinishSegment.OnPlayerEntered += ProcessLevelComplete;
        }

        private void UpdateDifficulty()
        {
            if (_model.CurrentDifficulty < _currentLevel.SpeedDifficulty)
            {
                _characterModel.ChangeSpeed(_currentLevel.SpeedDifficulty);
            }
            else
            {
                _characterModel.ChangeSpeed(_model.CurrentDifficulty);
            }

            if (_model.CurrentDifficulty < _currentLevel.CameraDifficulty)
            {
                _cameraModel.ChangeDifficulty(_currentLevel.CameraDifficulty);
            }
            else
            {
                _cameraModel.ChangeDifficulty(_model.CurrentDifficulty);
            }
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

        private void ProcessLevelLose()
        {
            _model.LoseLevel();

            _gameplayScreen.Hide();
            _cameraModel.FlyUpAsync().Forget();

            _audioService.CallVibration();
        }

        private void ProcessLevelComplete()
        {
            _model.CompleteLevel();

            ClearOldLevel();

            DeactivateLevel();
            ActivateNextLevel();

            BuildNextLevel();

            _characterModel.CenteringAsync(_currentLevel.StartSegment.SegmentCenter.position).Forget();
            _view.ChangeStyle();
            _audioService.PlaySound("Finish");
        }

        private void TakeCrystal()
        {
            //TODO Может быть проигран SFX.
            _model.AddCrystalByRun();
        }

        #region TUTORIALS
        private void CheckForTutorials()
        {
            if (_model.IsTutorialComplete)
            {
                return;
            }

            if (_model.CurrentLevelId == 0 & _currentLevel is TutorialLevelView)
            {
                var tutorLevel = _currentLevel as TutorialLevelView;
                if (tutorLevel)
                {
                    tutorLevel.TutorialInteractPoint.OnPlayerEntered += StartTapTutorial;
                    _gameplayScreen.TapButton.onClick.RemoveListener(ProcessClick);

                    _currentLevel.Crystals[0].OnTaken += StartCrystalsTutorial;
                }
            }
        }

        private void StartCrystalsTutorial()
        {
            StartCrystalsTutorialAsync().Forget();
            
            async UniTaskVoid StartCrystalsTutorialAsync()
            {
                _currentLevel.Crystals[0].OnTaken -= StartCrystalsTutorial;
                
                _characterModel.StopMove();
                _walletTutorial.IsTutorialDisplaying.Value = true;
                
                await _gameplayScreen.TapButton.OnClickAsync();
                
                _walletTutorial.IsTutorialDisplaying.Value = false;
                _characterModel.StartMove();
            }
        }

        private void StartTapTutorial()
        {
            StartTapTutorialAsync().Forget();

            async UniTaskVoid StartTapTutorialAsync()
            {
                var tutorLevel = _currentLevel as TutorialLevelView;
                tutorLevel.TutorialInteractPoint.OnPlayerEntered -= StartTapTutorial;

                _characterModel.StopMove();
                _gameplayScreen.TutorialView.Show();

                await _gameplayScreen.TapButton.OnClickAsync();

                _gameplayScreen.TutorialView.Hide();
                _characterModel.StartMove();
                ProcessClick();

                _gameplayScreen.TapButton.onClick.AddListener(ProcessClick);
                _model.IsTutorialComplete = true;
            }
        }
        #endregion

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

            _model.IsDisplaying.Unsubscribe(BuildLevel, true);
            _model.IsDisplaying.Unsubscribe(HideLevels, false);

            _model.StartupTrigger.Unsubscribe(StartGameplay);
            _model.ResetLevelTrigger.Unsubscribe(ResetLevel);

            _model.OnLevelChanged -= BuildLevel;

            _characterModel.IsFall.OnChangedToTrue -= ProcessLevelLose;
            _gameplayScreen.TapButton.onClick.RemoveListener(ProcessClick);
        }
    }
}