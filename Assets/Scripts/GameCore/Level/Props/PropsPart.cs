using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Level.Props
{
    public class PropsPart : MonoBehaviour
    {
        [SerializeField] public DecorPlace[] DecorPlaces;
        [SerializeField] public ObstaclePlace[] ObstaclePlaces;
        [NonSerialized] public List<GameObject> SpawnedProps;

#if UNITY_EDITOR
        [ContextMenu("Collect places")]
        public void CollectPlaces()
        {
            DecorPlaces = GetComponentsInChildren<DecorPlace>();
            ObstaclePlaces = GetComponentsInChildren<ObstaclePlace>();
        }
#endif
    }
}