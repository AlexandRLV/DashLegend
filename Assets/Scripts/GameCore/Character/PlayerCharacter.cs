using Framework.DI;
using GameCore.Input;
using UnityEngine;

namespace GameCore.Character
{
    public class PlayerCharacter : MonoBehaviour
    {
        [Inject] private readonly InputState _inputState;
        
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