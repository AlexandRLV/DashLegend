using System;
using Cysharp.Threading.Tasks;
using Framework.DI;
using UnityEngine;

namespace Framework.Initialization
{
    public class SceneInitializer : MonoBehaviour, IDisposable
    {
        [SerializeField] private InitializerBase[] _initializers;

        public InitializeOperationContainer Initialize()
        {
            var container = InitializeOperationContainer.Create();
            InitializeInternal(container.Value).Forget();
            return container;
        }
        
        private async UniTask InitializeInternal(InitializeOperation operation)
        {
            GameContainer.Current.Register(this);

            float i = 0;
            foreach (var initializer in _initializers)
            {
                GameContainer.Current.InjectToInstance(initializer);
                await initializer.Initialize();
                i++;
                operation.Progress = i / _initializers.Length;
            }

            operation.Progress = 1f;
            operation.IsDone = true;
        }

        public void Dispose()
        {
            foreach (var initializer in _initializers)
            {
                initializer.Dispose();
            }
        }
    }
}