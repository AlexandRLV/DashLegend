using Currency;

namespace LocalMessages
{
    public struct PlayerCurrencyChangedMessage
    {
        public CurrencyType Type;
        public int NewAmount;
    }
}