using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Lose.Model;
using UnityEngine;

namespace TapAndRun.Architecture.GameStates
{
    public class GameplayState : IGameState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ILevelsModel _levelsModel;

        public GameplayState(GameStateMachine stateMachine, ILevelsModel levelsModel)
        {
            _stateMachine = stateMachine;
            _levelsModel = levelsModel;
        }

        public UniTask EnterAsync(CancellationToken token)
        {
            _levelsModel.StartGameplay();

            _levelsModel.OnLevelFailed += ToLose;
            
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken token)
        {
            _levelsModel.OnLevelFailed -= ToLose;
            
            return UniTask.CompletedTask;
        }

        private void ToLose()
        {
            _stateMachine.ChangeStateAsync<LoseState>().Forget();
        }
    }
}