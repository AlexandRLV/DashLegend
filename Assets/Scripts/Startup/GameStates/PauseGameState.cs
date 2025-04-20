using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.GameStateMachine;
using Framework.GUI;
using GameCore.Character;
using GameCore.Level;
using GUI;

namespace Startup.GameStates
{
    public struct PauseGameStateData : IGameStateData { }
    
    public class PauseGameState : IGameState<PauseGameStateData>
    {
        [Inject] private readonly WindowsSystem _windowsSystem;
        [Inject] private readonly LevelGenerator _levelGenerator;
        [Inject] private readonly PlayerCharacter _playerCharacter;
        
        public UniTask OnEnter(PauseGameStateData data)
        {
            if (_windowsSystem.TryGetWindow(out GameHudWindow hudWindow))
                hudWindow.SetPlayState(false);
            
            _levelGenerator.Clear();
            _levelGenerator.StartSpawn(LevelGeneratorMode.Menu);
            _playerCharacter.MoveValues.IsAutoRun = true;
            _playerCharacter.Revive();
            return UniTask.CompletedTask;
        }

        public UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }
    }
}