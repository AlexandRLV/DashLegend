using Framework.DI;
using GameCore.Input;
using GameCore.Level;
using UnityEngine;

namespace GameCore.Character
{
    public class CharacterAutoJumper : MonoBehaviour, IInputSource
    {
        public bool JumpPressed { get; private set; }

        [SerializeField] private PlayerCharacter _playerCharacter;
        [SerializeField] private CharacterParameters _characterParameters;

        [Inject] private readonly InputState _inputState;

        private bool _jumpPressedSkipOneFrame;

        private void OnEnable()
        {
            GameContainer.Current.InjectToInstance(this);
            _inputState.RegisterInputSource(this);
        }

        private void OnDisable()
        {
            _inputState.UnregisterInputSource(this);
        }

        private void Update()
        {
            if (_jumpPressedSkipOneFrame)
            {
                _jumpPressedSkipOneFrame = false;
                return;
            }

            if (JumpPressed)
            {
                JumpPressed = false;
                return;
            }
            
            if (!_playerCharacter.MoveValues.IsAutoRun)
                return;

            if (!Physics.Raycast(transform.position + Vector3.up * _characterParameters.AutoJumpCheckUpOffset, transform.forward, out var hit, _characterParameters.AutoJumpCheckDistance))
                return;
            
            if (hit.colliderInstanceID == 0)
                return;
            
            if (hit.collider.GetComponent<Obstacle>() == null)
                return;

            JumpPressed = true;
            _jumpPressedSkipOneFrame = true;
        }
    }
}