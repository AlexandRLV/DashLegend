using Currency;
using Framework;
using LocalMessages;
using TMPro;
using UnityEngine;
using VContainer;

namespace GUI
{
    public class PlayerCurrencyDrawer : MonoBehaviour, IMessageListener<PlayerCurrencyChangedMessage>
    {
        [SerializeField] private CurrencyType _currencyType;
        [SerializeField] private TextMeshProUGUI _amountText;

        [Inject] private readonly LocalMessageBroker _localMessageBroker;
        [Inject] private readonly PlayerCurrencyController _playerCurrencyController;

        private void Start()
        {
            _localMessageBroker.Subscribe(this);
            Refresh();
        }

        private void OnDestroy()
        {
            _localMessageBroker.Unsubscribe(this);
        }

        public void OnMessage(in PlayerCurrencyChangedMessage message)
        {
            Refresh();
        }

        private void Refresh()
        {
            _amountText.text = _playerCurrencyController.GetCurrencyAmount(_currencyType).ToString();
        }
    }
}