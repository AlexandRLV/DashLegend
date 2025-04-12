using Framework.CharacterStateMachine;
using GameCore.Character.MoveStates;
using UnityEngine;

namespace GameCore.Character
{
    public class PlayerCharacter : MonoBehaviour
    {
        public CharacterMoveValues MoveValues;
        public bool IsDead;
        
        [SerializeField] public CharacterParameters Parameters;
        
        private bool _hasVisuals;
        private CharacterVisuals _visuals;

        private CharacterStateMachine<CharacterMoveStateBase, CharacterMoveStateType> _stateMachine;

        public void Initialize(CharacterVisuals visuals)
        {
            _visuals = visuals;
            _hasVisuals = _visuals != null;

            _stateMachine = new CharacterStateMachine<CharacterMoveStateBase, CharacterMoveStateType>();
            _stateMachine.States.Add(new CharacterMoveStateRun(this));
            _stateMachine.States.Add(new CharacterMoveStateJump(this));
            _stateMachine.States.Add(new CharacterMoveStateFall(this));
            
            _stateMachine.ForceSetState(CharacterMoveStateType.Run, true);
        }

        private void Update()
        {
            _stateMachine.Update();
            _stateMachine.CheckStates(true);
            
            if (_hasVisuals)
                _visuals.PlayAnimation(_stateMachine.CurrentState.AnimationType);
        }
    }
}