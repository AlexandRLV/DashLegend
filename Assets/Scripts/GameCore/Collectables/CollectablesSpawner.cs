using System;
using System.Collections.Generic;
using Framework;
using Framework.Extensions;
using Framework.Pools;
using GameCore.Level;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

namespace GameCore.Collectables
{
    public class CollectablesSpawner
    {
        public ReadOnlyList<BaseCollectable> ActiveCollectables => _activeCollectables;

        [Inject] private readonly IObjectResolver _container;
        
        private readonly Dictionary<Type, BaseCollectableHandler> _typeToHandler;
        private readonly Dictionary<CollectablePrefabType, BaseCollectable> _typeToPrefab;
        private readonly List<BaseCollectable> _activeCollectables;

        public CollectablesSpawner(CollectablesConfig config)
        {
            _typeToHandler = new Dictionary<Type, BaseCollectableHandler>();
            _typeToPrefab = new Dictionary<CollectablePrefabType, BaseCollectable>();
            _activeCollectables = new List<BaseCollectable>();
            
            foreach (var container in config.Collectables)
            {
                _typeToPrefab.Add(container.Item1, container.Item2);
            }
        }

        public void RegisterHandler(Type type, BaseCollectableHandler handler)
        {
            if (!_typeToHandler.TryAdd(type, handler))
                Debug.LogError($"[CollectablesController] Duplicate handler registration for type {type}");
        }

        public void UnregisterHandler(Type type)
        {
            _typeToHandler.Remove(type);
        }

        public void ProcessSpawnPart(LevelPart levelPart)
        {
            if (levelPart.SpawnedCollectables != null)
            {
                Debug.LogError("[CollectablesController] Didn't cleaned spawned collectables for level part");
                CleanupLevelPart(levelPart);
            }
            
            if (levelPart.CollectablePlaces == null || levelPart.CollectablePlaces.Length == 0)
                return;

            levelPart.SpawnedCollectables = ListPool<BaseCollectable>.Get();
            foreach (var place in levelPart.CollectablePlaces)
            {
                if (!_typeToPrefab.TryGetValue(place.Type, out var prefab))
                {
                    Debug.LogError($"[CollectablesController] Can't find prefab for type {place.Type.ToString()}");
                    continue;
                }

                var collectable = PrefabMonoPool<BaseCollectable>.GetPrefabInstance(prefab);
                _container.InjectGameObject(collectable.gameObject);
                
                collectable.transform.SetParent(place.transform);
                collectable.transform.MoveToLocalZero();
                levelPart.SpawnedCollectables.Add(collectable);
                _activeCollectables.Add(collectable);
            }
        }

        public void ProcessDespawnPart(LevelPart levelPart)
        {
            if (levelPart.SpawnedCollectables != null)
                CleanupLevelPart(levelPart);
        }

        public void CatchCollectable(BaseCollectable collectable)
        {
            var type = collectable.GetType();
            if (!_typeToHandler.TryGetValue(type, out var handler))
            {
                Debug.LogError($"[CollectablesController] No handler registered for type {type.Name}");
                return;
            }
            
            handler.HandleCollectable(collectable);
            collectable.gameObject.SetActive(false);
        }

        private void CleanupLevelPart(LevelPart levelPart)
        {
            if (levelPart.SpawnedCollectables == null)
                return;

            foreach (var collectable in levelPart.SpawnedCollectables)
            {
                collectable.Cleanup();
                _activeCollectables.Remove(collectable);
                PrefabMonoPool<BaseCollectable>.ReturnInstance(collectable);
            }
            
            levelPart.SpawnedCollectables.Clear();
            ListPool<BaseCollectable>.Release(levelPart.SpawnedCollectables);
            levelPart.SpawnedCollectables = null;
        }
    }
}