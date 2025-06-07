using GameCore.Boosters;
using UnityEngine;

namespace GameCore.Collectables.CollectableTypes
{
    public class BoosterCollectable : BaseCollectable
    {
        [SerializeField] public BoosterType Type;
        [SerializeField] public float Duration;
    }
}