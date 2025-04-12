using Framework.CharacterStateMachine;

namespace GameCore.Character.MoveStates
{
    public abstract class CharacterMoveStateBase : CharacterStateBase<CharacterMoveStateType>
    {
        public abstract AnimationType AnimationType { get; }
        
        protected PlayerCharacter Character;

        protected CharacterMoveStateBase(PlayerCharacter character)
        {
            Character = character;
        }
    }
}