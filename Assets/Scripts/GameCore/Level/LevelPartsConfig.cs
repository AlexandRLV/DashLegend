using System;
using Framework.Extensions;
using UnityEngine;

namespace GameCore.Level
{
    [Serializable]
    public class LevelPartContainer : ICollectionWithChanceItem
    {
        [field: SerializeField] public float Chance { get; private set; }
        [SerializeField] public LevelPart PartPrefab;
    }
    
    [CreateAssetMenu(fileName = nameof(LevelPartsConfig), menuName = "Configs/Level Parts")]
    public class LevelPartsConfig : ScriptableObject
    {
        [SerializeField] public LevelPartContainer[] Parts;
    }
}