using System;
using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.StateMachine;
using UnityEngine;

namespace Framework.Initialization
{
    public abstract class GameBootstrapperBase : MonoBehaviour
    {
        [SerializeField] private SceneInitializer _bootstrapSceneInitializer;

        protected void InitContainer()
        {
            var mainContainer = GameContainer.CreateNew();
            var stateMachine = new GameStateMachine();
            AddGameStates(stateMachine);
            mainContainer.Register(stateMachine);
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
            try
            {
                using var initializerOperation = _bootstrapSceneInitializer.Initialize();
                while (!operation.IsDone)
                {
                    await UniTask.Yield();
                    operation.Progress = initializerOperation.Progress;
                }
                
                operation.Progress = 1f;
                operation.IsDone = true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameBootstrapper] Error on initialization: {e}");
            }
        }

        protected abstract void AddGameStates(GameStateMachine stateMachine);
    }
}