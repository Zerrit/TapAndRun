using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.MainMenu.Model;
using TapAndRun.Services.Transition;
using UnityEngine;

namespace TapAndRun.Architecture.GameStates
{
    public class MainMenuState : IGameState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IMainMenuModel _mainMenuModel;
        private readonly ILevelsModel _levelsModel;
        private readonly ITransitionService _transitionService;

        public MainMenuState(GameStateMachine gameStateMachine, IMainMenuModel mainMenuModel,
            ILevelsModel levelsModel, ITransitionService transitionService)
        {
            _gameStateMachine = gameStateMachine;
            _mainMenuModel = mainMenuModel;
            _levelsModel = levelsModel;
            _transitionService = transitionService;
        }

        public UniTask EnterAsync(CancellationToken token)
        {
            _levelsModel.PrepeareCurrentLevel();
            _mainMenuModel.IsDisplaying.Value = true;

            _transitionService.TryEndTransition();

            _mainMenuModel.PlayTrigger.OnTriggered += ToGameplay;
            _mainMenuModel.SkinShopTrigger.OnTriggered += ToSkinShop;

            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken token)
        {
            _mainMenuModel.PlayTrigger.OnTriggered -= ToGameplay;
            _mainMenuModel.SkinShopTrigger.OnTriggered -= ToSkinShop;

            _mainMenuModel.IsDisplaying.Value = false;

            return UniTask.CompletedTask;
        }

        private void ToGameplay()
        {
            _gameStateMachine.ChangeStateAsync<GameplayState>().Forget();
        }

        private void ToSkinShop()
        {
            ToSkinShopAsync(_gameStateMachine.Cts.Token).Forget();
            
            async UniTaskVoid ToSkinShopAsync(CancellationToken token)
            {
                await _transitionService.ShowTransition(token);
                _levelsModel.RemoveTrigger.Trigger();

                _gameStateMachine.ChangeStateAsync<SkinShopState>().Forget();
            }
        }
    }
}