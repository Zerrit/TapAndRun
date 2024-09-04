using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.MainMenu.Model;
using TapAndRun.MVP.Settings.Model;
using TapAndRun.MVP.TransitionScreen.Model;
using UnityEngine;

namespace TapAndRun.Architecture.GameStates
{
    public class MainMenuState : IGameState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IMainMenuModel _mainMenuModel;
        private readonly ILevelsModel _levelsModel;
        private readonly ITransitionModel _transition;

        public MainMenuState(GameStateMachine gameStateMachine, IMainMenuModel mainMenuModel,
            ILevelsModel levelsModel, ITransitionModel transition)
        {
            _gameStateMachine = gameStateMachine;
            _mainMenuModel = mainMenuModel;
            _levelsModel = levelsModel;
            _transition = transition;
        }

        public UniTask EnterAsync(CancellationToken token)
        {
            _levelsModel.PrepeareCurrentLevel();
            _mainMenuModel.IsDisplaying.Value = true;

            _transition.CanFinish = true;

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
                _transition.CanFinish = false;
                _transition.StartTrigger.Trigger();
            
                await UniTask.WaitUntil(() => _transition.IsScreenHidden, cancellationToken: token);
            
                _gameStateMachine.ChangeStateAsync<SkinShopState>().Forget();
            }
        }
    }
}