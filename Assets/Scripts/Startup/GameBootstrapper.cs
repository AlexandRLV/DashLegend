using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.GUI;
using Framework.Initialization;
using Framework.StateMachine;
using GUI;
using Startup.GameStates;
using UnityEngine;

namespace Startup
{
    public class GameBootstrapper : GameBootstrapperBase
    {
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private UiRoot _uiRoot;
        
        private async void Awake()
        {
            InitContainer();
            
            GameContainer.Current.Register(_loadingScreen);
            GameContainer.Current.Register(_uiRoot);
            
            using var operation = Initialize();
            while (!operation.IsDone)
            {
                await UniTask.Yield();
                _loadingScreen.Progress = operation.Progress;
            }
            
            var stateMachine = GameContainer.Current.Resolve<GameStateMachine>();
            stateMachine.SwitchToState(new MainMenuGameStateData());
        }

        protected override void AddGameStates(GameStateMachine stateMachine)
        {
            stateMachine.AddGameState<MainMenuGameStateData, MainMenuGameState>();
            stateMachine.AddGameState<PlayGameStateData, PlayGameState>();
        }
    }
}