using System.Globalization;
using Framework.DI;
using Framework.GameStateMachine;
using Framework.GUI;
using GameCore.Input;
using GameCore.Level;
using Startup.GameStates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class GameHudWindow : WindowBase, IInputSource
    {
        public bool JumpPressed { get; private set; }
        
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private Button _jumpButton;

        [SerializeField] private GameObject[] _objectsInPause;
        [SerializeField] private GameObject[] _objectsInGame;

        [Inject] private readonly InputState _inputState;
        [Inject] private readonly LevelGenerator _levelGenerator;
        [Inject] private readonly GameStateMachine _gameStateMachine;

        private bool _isGameActive;
        private bool _jumpPressedSkipFrame;
        
        private void Start()
        {
            _settingsButton.onClick.AddListener(OnSettingsPressed);
            _pauseButton.onClick.AddListener(OnPausePressed);
            _jumpButton.onClick.AddListener(OnJumpPressed);
            _inputState.RegisterInputSource(this);
        }

        public override void Destroy()
        {
            _inputState.UnregisterInputSource(this);
            base.Destroy();
        }

        private void OnJumpPressed()
        {
            JumpPressed = true;
            _jumpPressedSkipFrame = true;
            
            if (!_gameStateMachine.IsInState<PlayGameStateData>())
                _gameStateMachine.SwitchToState<PlayGameStateData>();
        }

        public void SetPlayState(bool isGameActive)
        {
            _isGameActive = isGameActive;
            foreach (var target in _objectsInPause)
            {
                target.SetActive(!isGameActive);
            }
            
            foreach (var target in _objectsInGame)
            {
                target.SetActive(isGameActive);
            }
        }

        private void OnSettingsPressed()
        {
            // TODO: open settings
        }

        private void OnPausePressed()
        {
            _gameStateMachine.SwitchToState(new PauseGameStateData());
        }

        private void Update()
        {
            if (_isGameActive)
                _distanceText.text = Mathf.RoundToInt(_levelGenerator.PassedDistance).ToString(CultureInfo.InvariantCulture);
        }

        private void LateUpdate()
        {
            if (_jumpPressedSkipFrame)
            {
                _jumpPressedSkipFrame = false;
                return;
            }

            if (JumpPressed)
                JumpPressed = false;
        }
    }
}