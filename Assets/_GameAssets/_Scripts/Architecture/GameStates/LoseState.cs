using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Lose.Model;

namespace TapAndRun.Architecture.GameStates
{
    public class LoseState : IGameState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ILoseModel _loseModel;
        private readonly ILevelsModel _levelsModel;

        public LoseState(GameStateMachine stateMachine, ILoseModel loseModel, ILevelsModel levelsModel)
        {
            _stateMachine = stateMachine;
            _loseModel = loseModel;
            _levelsModel = levelsModel;
        }

        public UniTask EnterAsync(CancellationToken token)
        {
            _loseModel.IsDisplaying.Value = true;

            _loseModel.RestartTrigger.Subscribe(ToGameplay);
            _loseModel.HomeTrigger.Subscribe(ToMainMenu);

            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken token)
        {
            _loseModel.IsDisplaying.Value = false;

            _loseModel.RestartTrigger.Unsubscribe(ToGameplay);
            _loseModel.HomeTrigger.Unsubscribe(ToMainMenu);

            return UniTask.CompletedTask;
        }

        private void ToGameplay()
        {
            _levelsModel.ResetLevelTrigger.Trigger();

            _stateMachine.ChangeStateAsync<GameplayState>().Forget();
        }

        private void ToMainMenu()
        {
            _levelsModel.ResetLevelTrigger.Trigger();

            _stateMachine.ChangeStateAsync<MainMenuState>().Forget();
        }
    }
}