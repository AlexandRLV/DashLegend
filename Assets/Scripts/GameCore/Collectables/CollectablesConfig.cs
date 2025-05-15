using System;
using Currency;
using Framework;
using Framework.Sounds;
using UnityEngine;

namespace GameCore.Collectables
{
    [Serializable]
    public class CollectablesContainer : SerializableContainer<CollectableType, BaseCollectable> { }
    
    [Serializable]
    public class CurrencySoundContainer : SerializableContainer<CurrencyType, SoundType> { }
    
    [CreateAssetMenu(fileName = nameof(CollectablesConfig), menuName = "Configs/Collectables")]
    public class CollectablesConfig : ScriptableObject
    {
        [SerializeField] public CollectablesContainer[] Collectables;
        [SerializeField] public CurrencySoundContainer[] CurrencyCollectSounds;
    }
}