using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.Initialization;
using GameCore.Level;
using UnityEngine;

namespace Startup.LevelInitializers
{
    public class LevelGeneratorInitializer : InitializerBase
    {
        [SerializeField] private LevelGenerator _levelGeneratorPrefab;
        
        public override UniTask Initialize()
        {
            var levelGenerator = GameContainer.Current.InstantiateAndResolve(_levelGeneratorPrefab);
            GameContainer.Current.Register(levelGenerator);
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}