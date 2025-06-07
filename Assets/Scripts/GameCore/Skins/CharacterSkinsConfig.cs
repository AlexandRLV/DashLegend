using System;
using Framework;
using UnityEngine;

namespace GameCore.Skins
{
    [Serializable]
    public class SkinPriceContainer : SerializableContainer<SkinType, int> {}
    
    [CreateAssetMenu(fileName = "CharacterSkinsConfig", menuName = "Configs/Character Skins")]
    public class CharacterSkinsConfig : ScriptableObject
    {
        [SerializeField] public SkinPriceContainer[] SkinPrices;
    }
}