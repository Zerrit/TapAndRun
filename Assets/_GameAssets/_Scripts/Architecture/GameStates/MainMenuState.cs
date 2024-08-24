using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.MainMenu.Model;
using TapAndRun.MVP.Settings.Model;
using UnityEngine;

namespace TapAndRun.Architecture.GameStates
{
    public class MainMenuState : IGameState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IMainMenuModel _mainMenuModel;
        private readonly ILevelsModel _levelsModel;

        public MainMenuState(GameStateMachine gameStateMachine, IMainMenuModel mainMenuModel, ILevelsModel levelsModel)
        {
            _gameStateMachine = gameStateMachine;
            _mainMenuModel = mainMenuModel;
            _levelsModel = levelsModel;
        }

        public UniTask EnterAsync(CancellationToken token)
        {
            Debug.Log($"Запущен ----{typeof(MainMenuState)}----");

            _levelsModel.LoadLevel();
            // await UniTask.WaitUntil(()=> _levelModel.IsLevelPrepeared);
            
            _mainMenuModel.IsDisplaying.Value = true;
            
            _mainMenuModel.OnGameStarted += ToGameplay;
            
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken token)
        {
            _mainMenuModel.OnGameStarted -= ToGameplay;

            _mainMenuModel.IsDisplaying.Value = false;
            
            return UniTask.CompletedTask;
        }

        private void ToGameplay()
        {
            _gameStateMachine.ChangeStateAsync<GameplayState>().Forget();
        }
    }
}