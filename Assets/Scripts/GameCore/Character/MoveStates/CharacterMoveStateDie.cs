using Framework.DI;

namespace GameCore.Character.MoveStates
{
    public class CharacterMoveStateDie : CharacterMoveStateBase
    {
        public override CharacterMoveStateType Type => CharacterMoveStateType.Die;
        public override AnimationType AnimationType => AnimationType.Die;

        [Inject] private readonly RuntimeGameState _runtimeGameState;

        public CharacterMoveStateDie(PlayerCharacter character) : base(character)
        {
        }

        public override bool CanEnter(CharacterMoveStateType prevState) => Character.IsDead;
        public override bool CanExit(CharacterMoveStateType nextState) => !Character.IsDead;

        public override void OnEnter(CharacterMoveStateType prevState)
        {
            _runtimeGameState.SpeedMultiplier = 0f;
        }

        public override void OnExit(CharacterMoveStateType nextState)
        {
            _runtimeGameState.SpeedMultiplier = 1f;
        }
    }
}