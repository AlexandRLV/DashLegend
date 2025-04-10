using Cysharp.Threading.Tasks;
using Framework.StateMachine;

namespace Startup.GameStates
{
    public struct PlayGameStateData : IGameStateData
    {
        
    }
    
    public class PlayGameState : IGameState<PlayGameStateData>
    {
        public UniTask OnEnter(PlayGameStateData data)
        {
            return default;
        }

        public UniTask OnExit()
        {
            return default;
        }
    }
}