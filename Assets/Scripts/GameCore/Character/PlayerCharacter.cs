using UnityEngine;

namespace GameCore.Character
{
    public class PlayerCharacter : MonoBehaviour
    {
        public CharacterMoveValues MoveValues;
        
        [SerializeField] public CharacterParameters Parameters;
        
        private bool _hasVisuals;
        private CharacterVisuals _visuals;

        public void Initialize(CharacterVisuals visuals)
        {
            _visuals = visuals;
            _hasVisuals = _visuals != null;
        }
        
        // TODO: state machine
    }
}