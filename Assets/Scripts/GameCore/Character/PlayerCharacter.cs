using System;
using Currency;
using Framework;
using Framework.CharacterStateMachine;
using Framework.Extensions;
using Framework.Sounds;
using GameCore.Boosters;
using GameCore.Character.MoveStates;
using LocalMessages;
using UnityEngine;
using VContainer;

namespace GameCore.Character
{
    public class PlayerCharacter : MonoBehaviour, IMessageListener<PlayerDieMessage>, IDisposable
    {
        public bool IsDead;
        public CharacterMoveValues MoveValues;

        [SerializeField] public Rigidbody Rigidbody;
        [SerializeField] public CharacterParameters Parameters;

        [Inject] private readonly LocalMessageBroker _localMessageBroker;
        [Inject] private readonly PlayerCurrencyController _playerCurrencyController;
        [Inject] private readonly SoundSystem _soundSystem;
        [Inject] private readonly BoostersService _boostersService;
        [Inject] private readonly IObjectResolver _container;
        
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
            AddState(new CharacterMoveStateRun(this));
            AddState(new CharacterMoveStateJump(this));
            AddState(new CharacterMoveStateFall(this));
            AddState(new CharacterMoveStateDie(this));
            
            _stateMachine.ForceSetState(CharacterMoveStateType.Run, true);
            
            _localMessageBroker.Subscribe(this);
        }

        private void AddState(CharacterMoveStateBase state)
        {
            _container.Inject(state);
            _stateMachine.States.Add(state);
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

        public void ProcessObstacleHit()
        {
            if (_boostersService.IsBoosterActive(BoosterType.Shield))
                return;
            
            _soundSystem.PlaySound(SoundType.Hit);
            int lives = _playerCurrencyController.GetCurrencyAmount(CurrencyType.Lives);
            if (lives > 0)
            {
                // TODO: play effect
                _playerCurrencyController.AddCurrency(CurrencyType.Lives, -1);
                return;
            }
            
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