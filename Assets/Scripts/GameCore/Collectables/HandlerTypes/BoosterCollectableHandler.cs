using Framework.DI;
using Framework.Sounds;
using GameCore.Boosters;
using GameCore.Collectables.CollectableTypes;

namespace GameCore.Collectables.HandlerTypes
{
    public class BoosterCollectableHandler : BaseCollectableHandler<BoosterCollectable>
    {
        [Inject] private readonly BoostersService _boostersService;
        [Inject] private readonly CollectablesConfig _collectablesConfig;
        [Inject] private readonly SoundSystem _soundSystem;
        
        protected override void HandleCollectable(BoosterCollectable collectable)
        {
            _boostersService.ActivateBooster(collectable.Type, collectable.Duration);
            foreach (var collectSound in _collectablesConfig.BoosterCollectSounds)
            {
                if (collectSound.Item1 == collectable.Type)
                {
                    _soundSystem.PlaySound(collectSound.Item2);
                    return;
                }
            }
        }
    }
}