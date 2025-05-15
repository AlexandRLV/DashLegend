using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace GameCore.Level.Props
{
    [Serializable]
    public class DecorContainer : SerializableContainer<DecorType, GameObject[]> { }
    
    [Serializable]
    public class ObstacleContainer : SerializableContainer<ObstacleType, GameObject[]> { }
    
    [CreateAssetMenu(fileName = "LevelDecorConfig", menuName = "Configs/Level Decor")]
    public class LevelPropsConfig : ScriptableObject
    {
        [SerializeField] public DecorContainer[] Decors;
        [SerializeField] public ObstacleContainer[] Obstacles;

        public void FillDecorsToSpawn(DecorType type, List<GameObject> prefabs)
        {
            foreach (var container in Decors)
            {
                if ((type & container.Item1) == container.Item1)
                    prefabs.AddRange(container.Item2);
            }
        }
        
        public void FillObstaclesToSpawn(ObstacleType type, List<GameObject> prefabs)
        {
            foreach (var container in Obstacles)
            {
                if ((type & container.Item1) == container.Item1)
                    prefabs.AddRange(container.Item2);
            }
        }
    }
}