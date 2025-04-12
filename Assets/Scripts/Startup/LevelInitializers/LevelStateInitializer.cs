using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.Initialization;
using GameCore;
using UnityEngine;

namespace Startup.LevelInitializers
{
    public class LevelStateInitializer : InitializerBase
    {
        [SerializeField] private GameConfig _gameConfig;
        
        public override UniTask Initialize()
        {
            GameContainer.Current.Register(_gameConfig);
            GameContainer.Current.Register(GameContainer.Current.Create<RuntimeGameState>());
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}