using System;
using System.Collections.Generic;
using GameCore.Collectables;
using GameCore.Level.Props;
using UnityEngine;

namespace GameCore.Level
{
    public class LevelPart : MonoBehaviour
    {
        [SerializeField] public float HalfLength;
        [SerializeField] public CollectablePlace[] CollectablePlaces;
        [SerializeField] public PropsPart PropsPart;

        [NonSerialized] public List<BaseCollectable> SpawnedCollectables;

#if UNITY_EDITOR
        [ContextMenu("Add props part")]
        private void AddPropsPart()
        {
            if (PropsPart == null)
                PropsPart = GetComponent<PropsPart>();
            
            if (PropsPart == null)
                PropsPart = gameObject.AddComponent<PropsPart>();
            
            PropsPart.CollectPlaces();
        }
#endif
    }
}