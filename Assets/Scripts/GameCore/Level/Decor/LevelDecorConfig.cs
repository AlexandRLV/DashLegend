using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace GameCore.Level
{
    [Serializable]
    public class DecorContainer : SerializableContainer<DecorType, GameObject[]> { }
    
    [CreateAssetMenu(fileName = "LevelDecorConfig", menuName = "Configs/Level Decor")]
    public class LevelDecorConfig : ScriptableObject
    {
        [SerializeField] public DecorContainer[] Decors;

        public void FillDecorsToSpawn(DecorType type, List<GameObject> prefabs)
        {
            foreach (var container in Decors)
            {
                if ((type & container.Item1) == container.Item1)
                    prefabs.AddRange(container.Item2);
            }
        }
    }
}