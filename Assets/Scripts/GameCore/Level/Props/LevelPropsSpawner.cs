using System.Collections.Generic;
using Framework.Extensions;
using Framework.Pools;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

namespace GameCore.Level.Props
{
    public class LevelPropsSpawner
    {
        [Inject] private readonly IObjectResolver _container;
        [Inject] private readonly LevelPropsConfig _config;
        
        private readonly Queue<DecorPlace> _processingDecorPlaces = new();
        private readonly Queue<ObstaclePlace> _processingObstaclePlaces = new();
        private readonly List<GameObject> _processingProps = new();
        private readonly List<DecorPlace> _requestingDecorPlaces = new();
        private readonly List<ObstaclePlace> _requestingObstaclePlaces = new();

        public void ProcessSpawnPart(PropsPart propsPart)
        {
            if (propsPart.SpawnedProps != null)
            {
                Debug.LogError("[LevelPropsSpawner] Spawned props didn't cleaned on level part despawn");
                CleanupLevelPart(propsPart);
            }

            SpawnDecors(propsPart);
            SpawnObstacles(propsPart);
        }

        private void SpawnDecors(PropsPart propsPart)
        {
            if (propsPart.DecorPlaces == null || propsPart.DecorPlaces.Length == 0)
                return;
            
            propsPart.SpawnedProps ??= ListPool<GameObject>.Get();
            
            _processingDecorPlaces.Clear();
            foreach (var decorPlace in propsPart.DecorPlaces)
            {
                if (decorPlace == null)
                {
                    Debug.LogError($"[LevelPropsSpawner] Decor place is null, props part: {propsPart.gameObject.name}");
                    continue;
                }
                
                _processingDecorPlaces.Enqueue(decorPlace);
            }

            while (_processingDecorPlaces.TryDequeue(out var place))
            {
                if (place.MaxCount == 0) continue;
                
                _processingProps.Clear();
                _config.FillDecorsToSpawn(place.DecorType, _processingProps);
                if (_processingProps.Count == 0) continue;

                if (Random.value > place.TotalChance)
                    continue;
                
                for (int i = 0; i < place.MaxCount; i++)
                {
                    if (Random.value > place.ItemChance)
                        continue;
                    
                    var prefab = _processingProps.GetRandom();
                    var decor = PrefabGameObjectPool.GetPrefabInstance(prefab);
                    _container.InjectGameObject(decor);
                    
                    decor.transform.SetParent(place.transform);
                    decor.transform.MoveToLocalZero(changeScale: false);
                    if (place.RandomRotation)
                        decor.transform.rotation = Quaternion.Euler(0f, Random.value * 360f, 0f);
                    
                    propsPart.SpawnedProps.Add(decor);

                    var offset = Random.insideUnitCircle * place.Radius;
                    decor.transform.localPosition += new Vector3(offset.x, 0f, offset.y);
                    
                    _requestingDecorPlaces.Clear();
                    decor.GetComponentsInChildren(_requestingDecorPlaces);
                    foreach (var decorPlace in _requestingDecorPlaces)
                    {
                        if (decorPlace == null)
                        {
                            Debug.LogError($"[LevelPropsSpawner] Decor place is null, parent part: {decor.gameObject.name}");
                            continue;
                        }
                        _processingDecorPlaces.Enqueue(decorPlace);
                    }
                }
            }
            
            _processingProps.Clear();
            _processingDecorPlaces.Clear();
            _requestingDecorPlaces.Clear();
        }

        private void SpawnObstacles(PropsPart propsPart)
        {
            if (propsPart.ObstaclePlaces == null || propsPart.ObstaclePlaces.Length == 0)
                return;
            
            propsPart.SpawnedProps ??= ListPool<GameObject>.Get();
            
            _processingObstaclePlaces.Clear();
            foreach (var place in propsPart.ObstaclePlaces)
            {
                _processingObstaclePlaces.Enqueue(place);
            }

            while (_processingObstaclePlaces.TryDequeue(out var place))
            {
                _processingProps.Clear();
                _config.FillObstaclesToSpawn(place.Type, _processingProps);
                if (_processingProps.Count == 0) continue;

                var prefab = _processingProps.GetRandom();
                var obstacle = PrefabGameObjectPool.GetPrefabInstance(prefab);
                _container.InjectGameObject(obstacle);
                
                obstacle.transform.SetParent(place.transform);
                obstacle.transform.MoveToLocalZero(changeScale: false);
                propsPart.SpawnedProps.Add(obstacle);
                
                _requestingObstaclePlaces.Clear();
                obstacle.GetComponentsInChildren(_requestingObstaclePlaces);
                foreach (var childPlace in _requestingObstaclePlaces)
                {
                    _processingObstaclePlaces.Enqueue(childPlace);
                }
            }
            
            _processingProps.Clear();
            _processingObstaclePlaces.Clear();
            _requestingObstaclePlaces.Clear();
        }

        public void ProcessDespawnPart(PropsPart propsPart)
        {
            if (propsPart.SpawnedProps != null)
                CleanupLevelPart(propsPart);
        }

        private static void CleanupLevelPart(PropsPart propsPart)
        {
            foreach (var spawnedDecor in propsPart.SpawnedProps)
            {
                PrefabGameObjectPool.ReturnInstance(spawnedDecor);
            }
            
            propsPart.SpawnedProps.Clear();
            ListPool<GameObject>.Release(propsPart.SpawnedProps);
            propsPart.SpawnedProps = null;
        }
    }
}