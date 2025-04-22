using Currency;
using Framework.DI;
using GameCore.Collectables.CollectableTypes;

namespace GameCore.Collectables.HandlerTypes
{
    public class CurrencyCollectableHandler : BaseCollectableHandler<CurrencyCollectable>
    {
        [Inject] private readonly PlayerCurrencyController _playerCurrencyController;
        
        protected override void HandleCollectable(CurrencyCollectable collectable)
        {
            _playerCurrencyController.AddCurrency(collectable.Type, collectable.Amount);
        }
    }
}