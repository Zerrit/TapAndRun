using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Skins_Shop.Model;
using TapAndRun.MVP.TransitionScreen.Model;

namespace TapAndRun.Architecture.GameStates
{
    public class SkinShopState : IGameState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ISkinShopModel _skinShopModel;
        private readonly ITransitionModel _transition;
        private readonly ILevelsModel _levelsModel;

        public SkinShopState(GameStateMachine gameStateMachine, ISkinShopModel skinShopModel,
            ITransitionModel transition, ILevelsModel levelsModel)
        {
            _gameStateMachine = gameStateMachine;
            _skinShopModel = skinShopModel;
            _transition = transition;
            _levelsModel = levelsModel;
        }

        public async UniTask EnterAsync(CancellationToken token)
        {
            _levelsModel.RemoveTrigger.Trigger();
            _skinShopModel.IsDisplaying.Value = true;

            await UniTask.WaitUntil(() => _skinShopModel.IsAssortmentPrepeared, cancellationToken: token);

            _transition.CanFinish = true;

            _skinShopModel.BackTrigger.OnTriggered += ToMainMenu;
        }

        public async UniTask ExitAsync(CancellationToken token)
        {
            _skinShopModel.BackTrigger.OnTriggered -= ToMainMenu;

            _skinShopModel.IsDisplaying.Value = false;
        }

        private void ToMainMenu()
        {
            ToMainMenuAsync(_gameStateMachine.Cts.Token).Forget();

            async UniTaskVoid ToMainMenuAsync(CancellationToken token)
            {
                _transition.CanFinish = false;
                _transition.StartTrigger.Trigger();

                await UniTask.WaitUntil(() => _transition.IsScreenHidden, cancellationToken: token);

                _gameStateMachine.ChangeStateAsync<MainMenuState>().Forget();
            }
        }
    }
}