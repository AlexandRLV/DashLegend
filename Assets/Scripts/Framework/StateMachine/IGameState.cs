using Cysharp.Threading.Tasks;

namespace Framework.StateMachine
{
    public interface IGameStateData { }

    public interface IGameState
    {
        public UniTask OnExit();
    }
    
    public interface IGameState<in T> : IGameState where T : struct, IGameStateData
    {
        public UniTask OnEnter(T data);
    }
}