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
    public struct PlayGameStateData : IGameStateData { }
    
    public class PlayGameState : IGameState<PlayGameStateData>
    {
        [Inject] private readonly WindowsSystem _windowsSystem;
        [Inject] private readonly LevelGenerator _levelGenerator;
        [Inject] private readonly PlayerSpawnService _playerSpawnService;
        [Inject] private readonly GameController _gameController;
        [Inject] private readonly SoundSystem _soundSystem;
        
        public UniTask OnEnter(PlayGameStateData data)
        {
            if (_windowsSystem.TryGetWindow(out GameHudWindow hudWindow))
                hudWindow.SetPlayState(true);
            
            if (_playerSpawnService.Character.IsDead)
            {
                _levelGenerator.Clear();
                _playerSpawnService.Character.Revive();
            }
            
            _soundSystem.PlaySound(SoundType.StartGame);
            _soundSystem.PlayMusic(MusicType.Game);
            _levelGenerator.StartSpawn(LevelGeneratorMode.Game);
            _playerSpawnService.Character.MoveValues.IsAutoRun = false;
            _gameController.ResetInGameState(true);
            
            return UniTask.CompletedTask;
        }

        public UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }
    }
}