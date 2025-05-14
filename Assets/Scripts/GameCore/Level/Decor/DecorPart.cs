using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Level
{
    public class DecorPart : MonoBehaviour
    {
        [SerializeField] public DecorPlace[] DecorPlaces;
        [NonSerialized] public List<GameObject> SpawnedDecors;

#if UNITY_EDITOR
        [ContextMenu("Collect places")]
        public void CollectPlaces()
        {
            DecorPlaces = GetComponentsInChildren<DecorPlace>();
        }
#endif
    }
}