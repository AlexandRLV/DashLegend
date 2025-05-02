using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.Initialization;
using GameCore.CommonShadow;
using UnityEngine;

namespace Startup.LevelInitializers
{
    public class CommonLevelInitializer : InitializerBase
    {
        [SerializeField] private ShadowConfig _shadowConfig;
        
        public override UniTask Initialize()
        {
            GameContainer.Current.Register(_shadowConfig);
            GameContainer.Current.CreateAndRegister<ShadowController>();
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}