using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.Initialization;
using GameCore.Collectables;
using GameCore.Collectables.HandlerTypes;
using GameCore.Level;
using UnityEngine;

namespace Startup.LevelInitializers
{
    public class LevelGeneratorInitializer : InitializerBase
    {
        [SerializeField] private CollectablesConfig _collectablesConfig;
        [SerializeField] private LevelGenerator _levelGeneratorPrefab;
        
        public override UniTask Initialize()
        {
            var levelGenerator = GameContainer.Current.InstantiateAndResolve(_levelGeneratorPrefab);
            GameContainer.Current.Register(levelGenerator);
            
            GameContainer.Current.Register(_collectablesConfig);
            GameContainer.Current.CreateAndRegister<CollectablesController>();
            GameContainer.Current.CreateAndRegister<CurrencyCollectableHandler>();
            
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}