using Currency;
using UnityEngine;

namespace GameCore.Collectables.CollectableTypes
{
    public class CurrencyCollectable : BaseCollectable
    {
        [SerializeField] public CurrencyType Type;
        [SerializeField] public int Amount;
    }
}