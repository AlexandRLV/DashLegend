using System;
using Cysharp.Threading.Tasks;
using Framework.DI;
using UnityEngine;

namespace Framework.Initialization
{
    public abstract class GameBootstrapperBase : MonoBehaviour
    {
        [SerializeField] private SceneInitializer _bootstrapSceneInitializer;

        private async void Awake()
        {
            try
            {
                await OnAwake();
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameBootstrapper] Error on initialization: {e}");
            }
        }

        protected void InitContainer()
        {
            GameContainer.CreateNew();
        }

        protected void InitStateMachine()
        {
            var stateMachine = new GameStateMachine.GameStateMachine();
            GameContainer.Current.Register(stateMachine);
            AddGameStates(stateMachine);
        }
        
        protected InitializeOperationContainer Initialize()
        {
            DontDestroyOnLoad(gameObject);
            var container = InitializeOperationContainer.Create();
            InitializeInternal(container.Operation).Forget();
            return container;
        }

        private async UniTask InitializeInternal(InitializeOperation operation)
        {
            using var initializerOperation = _bootstrapSceneInitializer.Initialize();
            while (!initializerOperation.IsDone)
            {
                await UniTask.Yield();
                operation.Progress = initializerOperation.Progress;
            }
                
            operation.Progress = 1f;
            operation.IsDone = true;
        }

        protected abstract UniTask OnAwake();
        protected abstract void AddGameStates(GameStateMachine.GameStateMachine stateMachine);
    }
}