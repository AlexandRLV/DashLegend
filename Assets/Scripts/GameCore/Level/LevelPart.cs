using System;
using System.Collections.Generic;
using GameCore.Collectables;
using UnityEngine;

namespace GameCore.Level
{
    [Serializable]
    public class CollectablePlace
    {
        [SerializeField] public CollectableType Type;
        [SerializeField] public Transform Position;
    }
    
    public class LevelPart : MonoBehaviour
    {
        [SerializeField] public float HalfLength;
        [SerializeField] public CollectablePlace[] CollectablePlaces;

        [NonSerialized] public List<BaseCollectable> SpawnedCollectables;
    }
}