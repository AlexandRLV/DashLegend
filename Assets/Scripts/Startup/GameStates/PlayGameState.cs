using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.GameStateMachine;
using Framework.GUI;
using GameCore.Character;
using GameCore.Level;
using GUI;

namespace Startup.GameStates
{
    public struct PlayGameStateData : IGameStateData { }
    
    public class PlayGameState : IGameState<PlayGameStateData>
    {
        [Inject] private readonly WindowsSystem _windowsSystem;
        [Inject] private readonly LevelGenerator _levelGenerator;
        [Inject] private readonly PlayerCharacter _playerCharacter;
        
        public UniTask OnEnter(PlayGameStateData data)
        {
            if (_windowsSystem.TryGetWindow(out GameHudWindow hudWindow))
                hudWindow.SetPlayState(true);
            
            if (_playerCharacter.IsDead)
            {
                _levelGenerator.Clear();
                _playerCharacter.Revive();
            }
            
            _levelGenerator.StartSpawn(LevelGeneratorMode.Game);
            _playerCharacter.MoveValues.IsAutoRun = false;
            return UniTask.CompletedTask;
        }

        public UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }
    }
}