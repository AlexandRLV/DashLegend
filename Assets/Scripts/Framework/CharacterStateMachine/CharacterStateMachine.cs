using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.CharacterStateMachine
{
    public class CharacterStateMachine<TStateBase, TStateType>
        where TStateBase : CharacterStateBase<TStateType>
        where TStateType : Enum
    {
        public TStateBase CurrentState;
        public readonly List<TStateBase> States = new();
		
        private TStateType _currentType;
        private readonly EqualityComparer<TStateType> _comparer = EqualityComparer<TStateType>.Default;

        public void Update()
        {
            CurrentState.Update();
        }

        public void CheckStates(bool debug = false)
        {
            foreach (var nextState in States)
            {
                var nextType = nextState.Type;
                if (_comparer.Equals(nextType, _currentType)) continue;
				
                if (!nextState.CanEnter(_currentType)) continue;
                if (!CurrentState.CanExit(nextState.Type)) continue;

                if (debug) Debug.Log($"Switching state to {nextType.ToString()}");

                CurrentState.OnExit(nextState.Type);
                nextState.OnEnter(_currentType);
                CurrentState = nextState;
                _currentType = nextType;
                break;
            }
        }

        public void ForceSetState(TStateType stateType, bool debug = false)
        {
            if (CurrentState != null && _comparer.Equals(CurrentState.Type, stateType))
                return;
			
            foreach (var state in States)
            {
                if (!_comparer.Equals(stateType, state.Type)) continue;

                if (CurrentState != null)
                {
                    CurrentState.OnExit(stateType);
                    state.OnEnter(_currentType);
                }
                else
                {
                    state.OnEnter(default);
                }
				
                if (debug) Debug.Log($"Switching state to {stateType.ToString()}");

                CurrentState = state;
                _currentType = stateType;
            }
        }   
    }
}