using System;
using UnityEngine;

namespace GameCore.Collectables
{
    [Serializable]
    public class CollectablesContainer
    {
        [SerializeField] public CollectableType Type;
        [SerializeField] public BaseCollectable Prefab;
    }
    
    [CreateAssetMenu(fileName = nameof(CollectablesConfig), menuName = "Configs/Collectables")]
    public class CollectablesConfig : ScriptableObject
    {
        [SerializeField] public CollectablesContainer[] Collectables;
    }
}