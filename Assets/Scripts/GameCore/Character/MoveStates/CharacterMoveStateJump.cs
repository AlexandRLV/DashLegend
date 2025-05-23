﻿using Framework.DI;
using Framework.Extensions;
using Framework.Sounds;
using GameCore.Input;
using UnityEngine;

namespace GameCore.Character.MoveStates
{
    public class CharacterMoveStateJump : CharacterMoveStateBase
    {
        public override CharacterMoveStateType Type => CharacterMoveStateType.Jump;
        public override AnimationType AnimationType => AnimationType.Jump;

        [Inject] private readonly InputState _inputState;
        [Inject] private readonly SoundSystem _soundSystem;

        public CharacterMoveStateJump(PlayerCharacter character) : base(character)
        {
        }

        public override bool CanEnter(CharacterMoveStateType prevState) => prevState == CharacterMoveStateType.Run && _inputState.JumpPressed;
        public override bool CanExit(CharacterMoveStateType nextState) => nextState == CharacterMoveStateType.Fall && Character.MoveValues.JumpTimer >= Character.Parameters.JumpUpTime;

        public override void OnEnter(CharacterMoveStateType prevState)
        {
            _soundSystem.PlaySound(SoundType.Jump);
            Character.MoveValues.JumpTimer = 0f;
        }

        public override void Update()
        {
            Character.MoveValues.JumpTimer += GameTime.DeltaTime;
            float t = Character.MoveValues.JumpTimer / Character.Parameters.TotalJumpTime;
            float y = Mathf.Lerp(Character.MoveValues.StartJumpY, Character.MoveValues.EndJumpY, Character.Parameters.JumpCurve.Evaluate(t));
            Character.Rigidbody.SetYPosition(y);
        }
    }
}