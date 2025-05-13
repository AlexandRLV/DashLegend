using Currency;
using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.Initialization;
using Framework.Sounds;
using GameCore;
using UnityEngine;

namespace Startup.ServicesInitializers
{
    public class GameServicesInitializer : InitializerBase
    {
        [SerializeField] private SoundSystem _soundSystem;
        
        public override UniTask Initialize()
        {
            GameContainer.Current.CreateAndRegister<GameTime>();
            GameContainer.Current.CreateAndRegister<PlayerCurrencyController>();

            var soundsSystem = Instantiate(_soundSystem);
            GameContainer.Current.InjectToInstance(soundsSystem);
            DontDestroyOnLoad(soundsSystem.gameObject);
            GameContainer.Current.Register(soundsSystem);
            
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}