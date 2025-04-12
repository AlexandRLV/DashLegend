using Cysharp.Threading.Tasks;
using Framework.Initialization;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Framework
{
    public class SceneLoader
    {
        public InitializeOperationContainer LoadScene(string sceneName)
        {
            var container = InitializeOperationContainer.Create();
            LoadSceneInternal(sceneName, container.Operation).Forget();
            return container;
        }

        private async UniTask LoadSceneInternal(string sceneName, InitializeOperation operation)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            if (asyncOperation == null) return;
            
            while (!asyncOperation.isDone)
            {
                operation.Progress = asyncOperation.progress;
                await UniTask.Yield();
            }

            operation.Progress = 1f;
            operation.IsDone = true;
        }

        public bool TryGetActiveSceneInitializer(out SceneInitializer initializer)
        {
            using var rootObjectsPooled = ListPool<GameObject>.Get(out var rootObjects);
            
            var scene = SceneManager.GetActiveScene();
            scene.GetRootGameObjects(rootObjects);
            
            foreach (var gameObject in rootObjects)
            {
                if (gameObject.TryGetComponent(out SceneInitializer levelInitializer))
                {
                    initializer = levelInitializer;
                    return true;
                }
            }

            initializer = null;
            return false;
        }
    }
}