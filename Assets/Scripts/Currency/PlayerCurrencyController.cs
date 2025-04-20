using System.Collections.Generic;
using Framework;
using Framework.DI;
using LocalMessages;

namespace Currency
{
    public class PlayerCurrencyController
    {
        [Inject] private readonly LocalMessageBroker _localMessageBroker;
        
        private readonly Dictionary<CurrencyType, int> _currencyAmounts = new();

        public void AddCurrency(CurrencyType type, int amount)
        {
            int currentAmount = GetCurrencyAmount(type);
            currentAmount += amount;
            _currencyAmounts[type] = currentAmount;

            var message = new PlayerCurrencyChangedMessage
            {
                Type = type,
                NewAmount = amount
            };
            _localMessageBroker.Trigger(message);
        }

        public int GetCurrencyAmount(CurrencyType type)
            => _currencyAmounts.GetValueOrDefault(type, 0);
    }
}