using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.Initialization;
using GameCore.Level;
using UnityEngine;

namespace Startup.LevelInitializers
{
    public class LevelGeneratorInitializer : InitializerBase
    {
        [SerializeField] private LevelPartsConfig _levelPartsConfig;
        [SerializeField] private LevelGenerator _levelGeneratorPrefab;
        
        public override UniTask Initialize()
        {
            var levelGenerator = GameContainer.Current.InstantiateAndResolve(_levelGeneratorPrefab);
            levelGenerator.StartSpawn(_levelPartsConfig);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}