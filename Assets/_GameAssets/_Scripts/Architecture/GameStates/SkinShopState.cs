using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.CharacterCamera.Model;
using TapAndRun.MVP.Skins_Shop.Model;
using TapAndRun.Services.Transition;

namespace TapAndRun.Architecture.GameStates
{
    public class SkinShopState : IGameState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ISkinShopModel _skinShopModel;
        private readonly ITransitionService _transitionService;

        public SkinShopState(GameStateMachine gameStateMachine, ISkinShopModel skinShopModel,
            ITransitionService transitionService)
        {
            _gameStateMachine = gameStateMachine;
            _skinShopModel = skinShopModel;
            _transitionService = transitionService;
        }

        public async UniTask EnterAsync(CancellationToken token)
        {
            _skinShopModel.IsDisplaying.Value = true;
            await UniTask.WaitUntil(() => _skinShopModel.IsAssortmentPrepeared, cancellationToken: token);
            _transitionService.TryEndTransition();

            _skinShopModel.BackTrigger.OnTriggered += ToMainMenu;
        }

        public UniTask ExitAsync(CancellationToken token)
        {
            _skinShopModel.BackTrigger.OnTriggered -= ToMainMenu;

            _skinShopModel.IsDisplaying.Value = false;

            return UniTask.CompletedTask;
        }

        private void ToMainMenu()
        {
            ToMainMenuAsync(_gameStateMachine.Cts.Token).Forget();

            async UniTaskVoid ToMainMenuAsync(CancellationToken token)
            {
                await _transitionService.ShowTransition(token);

                _gameStateMachine.ChangeStateAsync<MainMenuState>().Forget();
            }
        }
    }
}