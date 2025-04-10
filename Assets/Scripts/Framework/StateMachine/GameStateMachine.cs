using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Framework.DI;
using UnityEngine;

namespace Framework.StateMachine
{
    public class GameStateMachine
    {
        private IGameState _currentState;
        private readonly Dictionary<Type, IGameState> _states = new();

        public void AddGameState<TStateData, TState>()
            where TStateData : struct, IGameStateData
            where TState : IGameState<TStateData>
        {
            var dataType = typeof(TStateData);
            _states.Add(dataType, (IGameState)GameContainer.Current.Create(typeof(TState)));
        }

        public void SwitchToState<T>(T data, bool force = false) where T : struct, IGameStateData
        {
            SwitchToStateAsync(data, force).Forget();
        }
        
        private async UniTask SwitchToStateAsync<T>(T data, bool force = false) where T : struct, IGameStateData
        {
            var type = typeof(T);
            Debug.Log($"Switching to state {type.Name}");

            var targetState = _states[type];
            
            if (_currentState == targetState && !force)
            {
                Debug.Log($"Already in state {targetState}");
                return;
            }

            if (_currentState != null)
                await _currentState.OnExit();
            
            _currentState = targetState;
            if (_currentState is not IGameState<T> state)
            {
                Debug.LogError($"[GameStateMachine] Can't cast state {_currentState} to {typeof(T)}");
                return;
            }

            await state.OnEnter(data);
        }
    }
}