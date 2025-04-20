using Cysharp.Threading.Tasks;
using Framework;
using Framework.DI;
using Framework.GameStateMachine;
using Framework.GUI;
using Framework.Initialization;
using GUI;
using Startup.GameStates;
using UnityEngine;

namespace Startup
{
    public class GameBootstrapper : GameBootstrapperBase
    {
        private const string GameSceneName = "GameScene";
        private const float LevelInitializerMaxProgress = 0.4f;
        private const float SceneLoadMaxProgress = 0.7f;
        
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private UiRoot _uiRoot;
        [SerializeField] private Camera _uiCamera;

        protected override async UniTask OnAwake()
        {
            DontDestroyOnLoad(_uiRoot.gameObject);
            DontDestroyOnLoad(_uiCamera.gameObject);
            
            InitContainer();
            
            var stateMachine = new GameStateMachine();
            GameContainer.Current.Register(stateMachine);
            
            GameContainer.Current.Register(_loadingScreen);
            GameContainer.Current.Register(_uiRoot);

            _loadingScreen.IsActive = true;
            using (var operation = Initialize())
            {
                while (!operation.IsDone)
                {
                    await UniTask.Yield();
                    _loadingScreen.Progress = Mathf.Lerp(0f, LevelInitializerMaxProgress, operation.Progress);
                }
            }

            await UniTask.Delay(1000);
            
            var sceneLoader = GameContainer.Current.Resolve<SceneLoader>();
            using (var operation = sceneLoader.LoadScene(GameSceneName))
            {
                while (!operation.IsDone)
                {
                    await UniTask.Yield();
                    _loadingScreen.Progress = Mathf.Lerp(LevelInitializerMaxProgress, SceneLoadMaxProgress, operation.Progress);
                }
            }

            if (sceneLoader.TryGetActiveSceneInitializer(out var sceneInitializer))
            {
                using var operation = sceneInitializer.Initialize();
                while (!operation.IsDone)
                {
                    await UniTask.Yield();
                    _loadingScreen.Progress = Mathf.Lerp(SceneLoadMaxProgress, 1f, operation.Progress);
                }
            }
            
            _loadingScreen.IsActive = false;

            var windowsSystem = GameContainer.Current.Resolve<WindowsSystem>();
            windowsSystem.PushWindow<GameHudWindow>();
            
            AddGameStates(stateMachine);
            stateMachine.SwitchToState(new PauseGameStateData());
        }

        private void AddGameStates(GameStateMachine stateMachine)
        {
            stateMachine.AddGameState<PauseGameStateData, PauseGameState>();
            stateMachine.AddGameState<PlayGameStateData, PlayGameState>();
        }
    }
}