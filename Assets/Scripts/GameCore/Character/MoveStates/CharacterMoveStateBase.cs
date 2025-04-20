using Framework.CharacterStateMachine;
using Framework.DI;

namespace GameCore.Character.MoveStates
{
    public abstract class CharacterMoveStateBase : CharacterStateBase<CharacterMoveStateType>
    {
        public abstract AnimationType AnimationType { get; }
        
        protected readonly PlayerCharacter Character;

        protected CharacterMoveStateBase(PlayerCharacter character)
        {
            Character = character;
            GameContainer.Current.InjectToInstance(this);
        }
    }
}