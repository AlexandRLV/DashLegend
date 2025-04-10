using Cysharp.Threading.Tasks;
using Framework.StateMachine;

namespace Startup.GameStates
{
    public struct MainMenuGameStateData : IGameStateData
    {
        
    }
    
    public class MainMenuGameState : IGameState<MainMenuGameStateData>
    {
        public UniTask OnEnter(MainMenuGameStateData data)
        {
            return default;
        }

        public UniTask OnExit()
        {
            return default;
        }
    }
}