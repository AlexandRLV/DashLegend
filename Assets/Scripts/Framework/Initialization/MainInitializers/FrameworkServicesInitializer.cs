using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.GUI;
using Framework.MonoUpdate;
using UnityEngine;

namespace Framework.Initialization.MainInitializers
{
    public class FrameworkServicesInitializer : InitializerBase
    {
        [SerializeField] private GameWindows _gameWindows;
        
        public override UniTask Initialize()
        {
            GameContainer.Current.Register(new LocalMessageBroker());
            GameContainer.Current.Register(_gameWindows);
            GameContainer.Current.Register(GameContainer.Current.Create<WindowsSystem>());
            
            GameContainer.Current.Register(new SceneLoader());

            var monoUpdaterGO = new GameObject("MonoUpdater");
            var monoUpdater = monoUpdaterGO.AddComponent<MonoUpdater>();
            GameContainer.Current.Register(monoUpdater);
            
            DontDestroyOnLoad(monoUpdaterGO);
            
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}