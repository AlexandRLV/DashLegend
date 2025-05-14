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
        [SerializeField] public DecorPart DecorPart;

        [NonSerialized] public List<BaseCollectable> SpawnedCollectables;

#if UNITY_EDITOR
        [ContextMenu("Add decor part")]
        private void AddDecorPart()
        {
            if (DecorPart == null)
                DecorPart = GetComponent<DecorPart>();
            
            if (DecorPart == null)
                DecorPart = gameObject.AddComponent<DecorPart>();
            
            DecorPart.CollectPlaces();
        }
#endif
    }
}