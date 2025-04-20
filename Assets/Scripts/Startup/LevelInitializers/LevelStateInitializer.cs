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
            var gameController = GameContainer.Current.Create<GameController>();
            gameController.Initialize();
            GameContainer.Current.Register(gameController);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}