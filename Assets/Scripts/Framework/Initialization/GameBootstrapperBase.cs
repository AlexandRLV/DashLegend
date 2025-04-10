using System;
using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.StateMachine;
using UnityEngine;

namespace Framework.Initialization
{
    public abstract class GameBootstrapperBase : SceneInitializer
    {
        private async void Awake()
        {
            DontDestroyOnLoad(gameObject);

            try
            {
                var mainContainer = GameContainer.CreateNew();
                var stateMachine = new GameStateMachine();
                AddGameStates(stateMachine);
                mainContainer.Register(stateMachine);
                
                var operation = Initialize();
                while (!operation.IsDone)
                {
                    await UniTask.Yield();
                    // TODO: progress
                }
                
                OnInitialized();
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameBootstrapper] Error on initialization: {e}");
            }
        }

        protected abstract void AddGameStates(GameStateMachine stateMachine);
        protected abstract void OnInitialized();
    }
}