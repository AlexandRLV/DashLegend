using Cysharp.Threading.Tasks;
using Framework.GameStateMachine;
using Framework.GUI;
using Framework.Sounds;
using GameCore;
using GameCore.Character;
using GameCore.Level;
using GUI;
using VContainer;

namespace Startup.GameStates
{
    public struct PauseGameStateData : IGameStateData { }
    
    public class PauseGameState : IGameState<PauseGameStateData>
    {
        [Inject] private readonly WindowsSystem _windowsSystem;
        [Inject] private readonly LevelGenerator _levelGenerator;
        [Inject] private readonly PlayerSpawnService _playerSpawnService;
        [Inject] private readonly GameController _gameController;
        [Inject] private readonly SoundSystem _soundSystem;
        
        public UniTask OnEnter(PauseGameStateData data)
        {
            if (_windowsSystem.TryGetWindow(out GameHudWindow hudWindow))
                hudWindow.SetPlayState(false);
            
            _soundSystem.PlayMusic(MusicType.Game);
            _levelGenerator.Clear();
            _levelGenerator.StartSpawn(LevelGeneratorMode.Menu);
            _playerSpawnService.Character.MoveValues.IsAutoRun = true;
            _playerSpawnService.Character.Revive();
            _gameController.ResetInGameState(false);
            
            return UniTask.CompletedTask;
        }

        public UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }
    }
}