using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.Initialization;
using GameCore.Collectables;
using GameCore.Collectables.HandlerTypes;
using GameCore.Level;
using GameCore.Level.Props;
using UnityEngine;

namespace Startup.LevelInitializers
{
    public class LevelGeneratorInitializer : InitializerBase
    {
        [SerializeField] private CollectablesConfig _collectablesConfig;
        [SerializeField] private LevelGenerator _levelGeneratorPrefab;
        [SerializeField] private LevelPropsConfig _levelPropsConfig;
        
        public override UniTask Initialize()
        {
            GameContainer.Current.Register(_collectablesConfig);
            var collectablesSpawner = new CollectablesSpawner(_collectablesConfig);
            GameContainer.Current.Register(collectablesSpawner);

            var decorSpawner = new LevelPropsSpawner(_levelPropsConfig);
            GameContainer.Current.Register(decorSpawner);
            
            var levelGenerator = GameContainer.Current.InstantiateAndResolve(_levelGeneratorPrefab);
            GameContainer.Current.Register(levelGenerator);
            
            GameContainer.Current.CreateAndRegister<CurrencyCollectableHandler>();
            
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}