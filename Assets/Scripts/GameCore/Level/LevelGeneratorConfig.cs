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
    
    [CreateAssetMenu(menuName = "Configs/Level Parts")]
    public class LevelGeneratorConfig : ScriptableObject
    {
        [SerializeField] public float EmptyPartsSpawnTime;
        [SerializeField] public float CoverDistance;
        [SerializeField] public float DestroyDistance;
        [SerializeField] public LevelPartContainer[] EmptyParts;
        [SerializeField] public LevelPartContainer[] GameLevelParts;
        [SerializeField] public LevelPartContainer[] MenuLevelParts;
    }
}