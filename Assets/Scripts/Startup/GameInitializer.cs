using Framework.GameStateMachine;
using Framework.GUI;
using GameCore.Character;
using GUI;
using Startup.GameStates;
using UnityEngine;
using VContainer;

namespace Startup
{
    public class GameInitializer : MonoBehaviour
    {
        [Inject] private readonly WindowsSystem _windowsSystem;
        [Inject] private readonly GameStateMachine _gameStateMachine;
        [Inject] private readonly PlayerSpawnService _playerSpawnService;
        
        private void Start()
        {
            _playerSpawnService.Spawn();
            _windowsSystem.PushWindow<GameHudWindow>();
            
            _gameStateMachine.AddGameState<PauseGameStateData, PauseGameState>();
            _gameStateMachine.AddGameState<PlayGameStateData, PlayGameState>();
            _gameStateMachine.SwitchToState(new PauseGameStateData());
        }
    }
}