using Framework.DI;
using Framework.GameStateMachine;
using Framework.GUI;
using Startup.GameStates;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class GameOverWindow : WindowBase
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _playButton;

        [Inject] private readonly GameStateMachine _gameStateMachine;
        
        protected override void Start()
        {
            base.Start();
            _pauseButton.onClick.AddListener(OnPausePressed);
            _playButton.onClick.AddListener(OnPlayPressed);
        }

        private void OnPlayPressed()
        {
            Close();
            _gameStateMachine.SwitchToState<PlayGameStateData>(force: true);
        }

        private void OnPausePressed()
        {
            Close();
            _gameStateMachine.SwitchToState<PauseGameStateData>();
        }
    }
}