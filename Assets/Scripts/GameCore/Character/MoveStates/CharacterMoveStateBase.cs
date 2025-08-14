using Framework.CharacterStateMachine;
using VContainer;

namespace GameCore.Character.MoveStates
{
    public abstract class CharacterMoveStateBase : CharacterStateBase<CharacterMoveStateType>
    {
        public abstract AnimationType AnimationType { get; }

        [Inject] protected GameTime GameTime;
        
        protected readonly PlayerCharacter Character;

        protected CharacterMoveStateBase(PlayerCharacter character)
        {
            Character = character;
        }
    }
}