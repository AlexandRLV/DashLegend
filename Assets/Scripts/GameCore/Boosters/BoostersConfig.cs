using System;
using Framework;
using UnityEngine;

namespace GameCore.Boosters
{
    [Serializable]
    public class BoosterSpriteContainer : SerializableContainer<BoosterType, Sprite> { }
    
    [CreateAssetMenu(fileName = "BoostersConfig", menuName = "Configs/Boosters")]
    public class BoostersConfig : ScriptableObject
    {
        [SerializeField] public float MagnetRadius;
        [SerializeField] public float MagnetSpeed;

        [SerializeField] public BoosterSpriteContainer[] SpriteContainers;
    }
}