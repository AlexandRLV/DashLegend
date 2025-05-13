using System;
using System.Collections.Generic;
using Framework;
using Framework.DI;
using LocalMessages;
using UnityEngine;

namespace Currency
{
    public class PlayerCurrencyController : IInitializable
    {
        private const string CurrencyAmountPrefsKey = "PlayerCurrencyAmount_";
        
        [Inject] private readonly LocalMessageBroker _localMessageBroker;
        
        private readonly Dictionary<CurrencyType, int> _currencyAmounts = new();

        public void Initialize()
        {
            foreach (CurrencyType currencyType in Enum.GetValues(typeof(CurrencyType)))
            {
                string key = GetPrefsKey(currencyType);
                if (!PlayerPrefs.HasKey(key))
                {
                    _currencyAmounts[currencyType] = 0;
                    continue;
                }

                try
                {
                    _currencyAmounts[currencyType] = PlayerPrefs.GetInt(key);
                }
                catch
                {
                    _currencyAmounts[currencyType] = 0;
                }
            }
        }

        public void AddCurrency(CurrencyType type, int amount)
        {
            int currentAmount = GetCurrencyAmount(type);
            currentAmount += amount;
            _currencyAmounts[type] = currentAmount;
            
            PlayerPrefs.SetInt(GetPrefsKey(type), currentAmount);

            var message = new PlayerCurrencyChangedMessage
            {
                Type = type,
                NewAmount = amount
            };
            _localMessageBroker.Trigger(message);
        }

        public int GetCurrencyAmount(CurrencyType type)
            => _currencyAmounts.GetValueOrDefault(type, 0);

        private string GetPrefsKey(CurrencyType type)
        {
            return $"{CurrencyAmountPrefsKey}{((int)type).ToString()}";
        }
    }
}