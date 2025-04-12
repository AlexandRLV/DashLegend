namespace GameCore.Character.MoveStates
{
    public class CharacterMoveStateRun : CharacterMoveStateBase
    {
        public override CharacterMoveStateType Type => CharacterMoveStateType.Run;
        public override AnimationType AnimationType => AnimationType.Run;
        
        public CharacterMoveStateRun(PlayerCharacter character) : base(character)
        {
        }
    }
}