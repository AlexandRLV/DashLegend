using System;
using Framework;
using Framework.CharacterStateMachine;
using Framework.DI;
using Framework.Extensions;
using GameCore.Character.MoveStates;
using GameCore.Level;
using LocalMessages;
using UnityEngine;

namespace GameCore.Character
{
    public class PlayerCharacter : MonoBehaviour, IMessageListener<PlayerDieMessage>, IDisposable
    {
        public bool IsDead;
        public CharacterMoveValues MoveValues;

        [SerializeField] public Rigidbody Rigidbody;
        [SerializeField] public CharacterParameters Parameters;

        [Inject] private readonly LocalMessageBroker _localMessageBroker;
        
        private bool _hasVisuals;
        private CharacterVisuals _visuals;

        private CharacterStateMachine<CharacterMoveStateBase, CharacterMoveStateType> _stateMachine;

        public void Initialize(CharacterVisuals visuals, float startYPosition)
        {
            MoveValues = new CharacterMoveValues
            {
                StartJumpY = startYPosition,
                EndJumpY = startYPosition + Parameters.JumpHeight
            };
            Debug.Log($"Start y: {MoveValues.StartJumpY}");
            
            _visuals = visuals;
            _hasVisuals = _visuals != null;

            _stateMachine = new CharacterStateMachine<CharacterMoveStateBase, CharacterMoveStateType>();
            _stateMachine.States.Add(new CharacterMoveStateRun(this));
            _stateMachine.States.Add(new CharacterMoveStateJump(this));
            _stateMachine.States.Add(new CharacterMoveStateFall(this));
            _stateMachine.States.Add(new CharacterMoveStateDie(this));
            
            _stateMachine.ForceSetState(CharacterMoveStateType.Run, true);
            
            _localMessageBroker.Subscribe(this);
        }

        public void Revive()
        {
            _stateMachine.ForceSetState(CharacterMoveStateType.Run, true);
            IsDead = false;
            Rigidbody.SetYPosition(MoveValues.StartJumpY);
        }

        private void Update()
        {
            if (_hasVisuals)
            {
                _visuals.PlayAnimation(_stateMachine.CurrentState.AnimationType);
                _visuals.transform.SetPositionAndRotation(transform.position, transform.rotation);
            }
        }

        private void FixedUpdate()
        {
            _stateMachine.Update();
            _stateMachine.CheckStates(true);
        }

        public void OnTrigger(Collider other)
        {
            if (MoveValues.IsAutoRun) return;
            if (IsDead) return;
            if (other.GetComponent<Obstacle>() == null)
                return;
            
            IsDead = true;
            _localMessageBroker.TriggerEmpty<PlayerDeadMessage>();
        }

        public void OnMessage(in PlayerDieMessage message)
        {
            IsDead = true;
            _localMessageBroker.TriggerEmpty<PlayerDeadMessage>();
        }

        public void Dispose()
        {
            _localMessageBroker.Unsubscribe(this);
        }
    }
}