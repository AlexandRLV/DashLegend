using System.Collections.Generic;
using Framework.Extensions;
using Framework.Pools;
using UnityEngine;
using UnityEngine.Pool;

namespace GameCore.Level
{
    public class LevelDecorSpawner
    {
        private readonly Queue<DecorPlace> _processingPlaces;
        private readonly List<GameObject> _processingDecors;
        private readonly List<DecorPlace> _requestingPlaces;
        private readonly LevelDecorConfig _config;

        public LevelDecorSpawner(LevelDecorConfig config)
        {
            _processingPlaces = new Queue<DecorPlace>();
            _processingDecors = new List<GameObject>();
            _requestingPlaces = new List<DecorPlace>();
            _config = config;
        }

        public void ProcessSpawnPart(DecorPart decorPart)
        {
            if (decorPart.SpawnedDecors != null)
            {
                Debug.LogError("[LevelDecorSpawner] Spawned decors didn't cleaned on level part despawn");
                CleanupLevelPart(decorPart);
            }
            
            if (decorPart.DecorPlaces == null || decorPart.DecorPlaces.Length == 0)
                return;
            
            decorPart.SpawnedDecors = ListPool<GameObject>.Get();
            _processingPlaces.Clear();
            foreach (var decorPlace in decorPart.DecorPlaces)
            {
                _processingPlaces.Enqueue(decorPlace);
            }

            while (_processingPlaces.TryDequeue(out var place))
            {
                _processingDecors.Clear();
                _config.FillDecorsToSpawn(place.DecorType, _processingDecors);
                if (_processingDecors.Count == 0) continue;
                if (place.MaxCount == 0) continue;

                if (Random.value > place.TotalChance)
                    continue;
                
                for (int i = 0; i < place.MaxCount; i++)
                {
                    if (Random.value > place.ItemChance)
                        continue;
                    
                    var prefab = _processingDecors.GetRandom();
                    var decor = PrefabGameObjectPool.GetPrefabInstance(prefab);
                    decor.transform.SetParent(place.transform);
                    decor.transform.MoveToLocalZero(changeScale: false);
                    decorPart.SpawnedDecors.Add(decor);

                    var offset = Random.insideUnitCircle * place.Radius;
                    decor.transform.localPosition += new Vector3(offset.x, 0f, offset.y);
                    
                    _requestingPlaces.Clear();
                    decor.GetComponentsInChildren(_requestingPlaces);
                    foreach (var decorPlace in _requestingPlaces)
                    {
                        _processingPlaces.Enqueue(decorPlace);
                    }
                }
            }
        }

        public void ProcessDespawnPart(DecorPart decorPart)
        {
            if (decorPart.SpawnedDecors != null)
                CleanupLevelPart(decorPart);
        }

        private static void CleanupLevelPart(DecorPart decorPart)
        {
            foreach (var spawnedDecor in decorPart.SpawnedDecors)
            {
                PrefabGameObjectPool.ReturnInstance(spawnedDecor);
            }
            
            decorPart.SpawnedDecors.Clear();
            ListPool<GameObject>.Release(decorPart.SpawnedDecors);
            decorPart.SpawnedDecors = null;
        }
    }
}