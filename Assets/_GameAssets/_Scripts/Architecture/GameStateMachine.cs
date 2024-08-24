using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Architecture.GameStates;
using TapAndRun.Factories.GameStates;
using UnityEngine;

namespace TapAndRun.Architecture
{
    /// <summary>
    /// Машина состояний игры.
    /// </summary>
    public class GameStateMachine : IDisposable
    {
        private CancellationTokenSource _cts;

        private IGameState _currentState;
        private Dictionary<Type, IGameState> _states;

        private readonly IGameStateFactory _stateFactory;

        public GameStateMachine(IGameStateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }

        public void Initialize()
        {
            _cts = new CancellationTokenSource();

            _states = new Dictionary<Type, IGameState>()
            {
                [typeof(MainMenuState)] = _stateFactory.CreateState<MainMenuState>(),
                [typeof(GameplayState)] = _stateFactory.CreateState<GameplayState>(),
                [typeof(LoseState)] = _stateFactory.CreateState<LoseState>()
            };

            Debug.Log("GameStateMachine was initialized!");
        }

        /// <summary>
        /// Запускает машину состояний игры в указанное состояние.
        /// </summary>
        /// <typeparam name="T"> Тип стейта. </typeparam>
        public async UniTask StartMachineAsync<T>(CancellationToken cancellation) where T : IGameState
        {            
            Debug.Log("GameStateMachine was started!");
            
            if (_states.ContainsKey(typeof(T)))
            {
                _currentState = _states[typeof(T)];
                await _states[typeof(T)].EnterAsync(cancellation);
            }
            else throw new Exception($"Не найдено указанное состояние игры: {typeof(T)}");
        }

        /// <summary>
        /// Меняет стейт машины на новый.
        /// </summary>
        /// <typeparam name="T"> Тип стейта. </typeparam>
        public async UniTask ChangeStateAsync<T>() where T : IGameState
        {
            if (_states.ContainsKey(typeof(T)))
            {
                await _currentState.ExitAsync(_cts.Token);
                _currentState = _states[typeof(T)];
                await _states[typeof(T)].EnterAsync(_cts.Token);
            }
            else throw new Exception($"Не найдено указанное состояние игры: {typeof(T)}");
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}