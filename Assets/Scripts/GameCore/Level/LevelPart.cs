using System;
using System.Collections.Generic;
using GameCore.Collectables;
using UnityEngine;

namespace GameCore.Level
{
    public class LevelPart : MonoBehaviour
    {
        [SerializeField] public float HalfLength;
        [SerializeField] public CollectablePlace[] CollectablePlaces;

        [NonSerialized] public List<BaseCollectable> SpawnedCollectables;
    }
}