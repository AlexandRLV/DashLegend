using System;
using Cysharp.Threading.Tasks;
using Framework.DI;
using UnityEngine;

namespace Framework.Initialization
{
    public class SceneInitializer : MonoBehaviour, IDisposable
    {
        [SerializeField] private InitializerBase[] _initializers;

        public async UniTask Initialize()
        {
            GameContainer.Current.Register(this);
            
            // float currentProgress = loadingScreen.Progress; // TODO: make progress as framework
            // float progressStep = (1f - currentProgress) / _initializers.Length;
            
            foreach (var initializer in _initializers)
            {
                GameContainer.Current.InjectToInstance(initializer);
                await initializer.Initialize();
                // currentProgress += progressStep;
                // loadingScreen.Progress = currentProgress;
            }
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