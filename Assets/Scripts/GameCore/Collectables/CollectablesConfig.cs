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
    public class CurrencyCollectableContainer : SerializableContainer<CurrencyType, SoundType> { }
    
    [CreateAssetMenu(fileName = nameof(CollectablesConfig), menuName = "Configs/Collectables")]
    public class CollectablesConfig : ScriptableObject
    {
        [SerializeField] public CollectablesContainer[] Collectables;
        [SerializeField] public CurrencyCollectableContainer[] CurrencyCollectSounds;
    }
}