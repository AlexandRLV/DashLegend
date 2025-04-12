using Framework;
using Framework.CharacterStateMachine;
using Framework.DI;
using GameCore.Character.MoveStates;
using GameCore.Level;
using LocalMessages;
using UnityEngine;

namespace GameCore.Character
{
    public class PlayerCharacter : MonoBehaviour
    {
        public bool IsDead;
        public readonly CharacterMoveValues MoveValues = new();

        [SerializeField] public CharacterParameters Parameters;

        [Inject] private readonly LocalMessageBroker _localMessageBroker;
        
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
            _stateMachine.States.Add(new CharacterMoveStateDie(this));
            
            _stateMachine.ForceSetState(CharacterMoveStateType.Run, true);
        }

        private void Update()
        {
            _stateMachine.Update();
            _stateMachine.CheckStates(true);
            
            if (_hasVisuals)
            {
                _visuals.PlayAnimation(_stateMachine.CurrentState.AnimationType);
                _visuals.transform.SetPositionAndRotation(transform.position, transform.rotation);
            }
        }

        public void OnTrigger(Collider other)
        {
            if (other.GetComponent<Obstacle>() != null)
            {
                IsDead = true;
                _localMessageBroker.TriggerEmpty<PlayerDeadMessage>();
            }
        }
    }
}