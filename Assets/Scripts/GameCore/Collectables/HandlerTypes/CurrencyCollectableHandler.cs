using Currency;
using Framework.DI;
using Framework.Sounds;
using GameCore.Collectables.CollectableTypes;

namespace GameCore.Collectables.HandlerTypes
{
    public class CurrencyCollectableHandler : BaseCollectableHandler<CurrencyCollectable>
    {
        [Inject] private readonly PlayerCurrencyController _playerCurrencyController;
        [Inject] private readonly SoundSystem _soundSystem;
        [Inject] private readonly CollectablesConfig _collectablesConfig;
        
        protected override void HandleCollectable(CurrencyCollectable collectable)
        {
            _playerCurrencyController.AddCurrency(collectable.Type, collectable.Amount);
            foreach (var collectSound in _collectablesConfig.CurrencyCollectSounds)
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