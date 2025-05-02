using Currency;
using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.Initialization;
using GameCore;

namespace Startup.ServicesInitializers
{
    public class GameServicesInitializer : InitializerBase
    {
        public override UniTask Initialize()
        {
            GameContainer.Current.CreateAndRegister<GameTime>();
            GameContainer.Current.CreateAndRegister<PlayerCurrencyController>();
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}