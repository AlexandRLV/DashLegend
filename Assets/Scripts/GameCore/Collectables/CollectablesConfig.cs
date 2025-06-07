using System;
using Currency;
using Framework;
using Framework.Sounds;
using GameCore.Boosters;
using UnityEngine;

namespace GameCore.Collectables
{
    [Serializable]
    public class CollectablesContainer : SerializableContainer<CollectablePrefabType, BaseCollectable> { }
    
    [Serializable]
    public class CurrencySoundContainer : SerializableContainer<CurrencyType, SoundType> { }
    
    [Serializable]
    public class BoosterSoundContainer : SerializableContainer<BoosterType, SoundType> { }
    
    [CreateAssetMenu(fileName = nameof(CollectablesConfig), menuName = "Configs/Collectables")]
    public class CollectablesConfig : ScriptableObject
    {
        [SerializeField] public CollectablesContainer[] Collectables;
        [SerializeField] public CurrencySoundContainer[] CurrencyCollectSounds;
        [SerializeField] public BoosterSoundContainer[] BoosterCollectSounds;
    }
}