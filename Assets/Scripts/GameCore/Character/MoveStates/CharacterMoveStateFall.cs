using Framework.Extensions;
using UnityEngine;

namespace GameCore.Character.MoveStates
{
    public class CharacterMoveStateFall : CharacterMoveStateBase
    {
        public override CharacterMoveStateType Type => CharacterMoveStateType.Fall;
        public override AnimationType AnimationType => AnimationType.Fall;
        
        public CharacterMoveStateFall(PlayerCharacter character) : base(character)
        {
        }

        public override bool CanEnter(CharacterMoveStateType prevState) => prevState == CharacterMoveStateType.Jump;
        public override bool CanExit(CharacterMoveStateType nextState) => Character.MoveValues.JumpTimer >= Character.Parameters.TotalJumpTime;
        
        public override void OnExit(CharacterMoveStateType nextState)
        {
            Character.Rigidbody.SetYPosition(Character.MoveValues.StartJumpY);
        }

        public override void Update()
        {
            Character.MoveValues.JumpTimer += Time.deltaTime;
            float t = Character.MoveValues.JumpTimer / Character.Parameters.TotalJumpTime;
            float y = Mathf.Lerp(Character.MoveValues.StartJumpY, Character.MoveValues.EndJumpY, Character.Parameters.JumpCurve.Evaluate(t));
            Character.Rigidbody.SetYPosition(y);
        }
    }
}