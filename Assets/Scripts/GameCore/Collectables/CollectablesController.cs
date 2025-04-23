using System;
using System.Collections.Generic;
using Framework;
using Framework.DI;
using Framework.Extensions;
using Framework.Pools;
using GameCore.Level;
using LocalMessages;
using UnityEngine;
using UnityEngine.Pool;

namespace GameCore.Collectables
{
    public class CollectablesController : IInitializable, IDisposable,
        IMessageListener<LevelPartPlacedMessage>, IMessageListener<LevelPartWillBeRemovedMessage>, IMessageListener<CatchCollectableMessage>
    {
        [Inject] private readonly CollectablesConfig _collectablesConfig;
        [Inject] private readonly LocalMessageBroker _localMessageBroker;

        private Dictionary<Type, BaseCollectableHandler> _typeToHandler;
        private Dictionary<CollectableType, BaseCollectable> _typeToPrefab;

        public void Initialize()
        {
            _typeToHandler = new Dictionary<Type, BaseCollectableHandler>();
            _typeToPrefab = new Dictionary<CollectableType, BaseCollectable>();
            foreach (var container in _collectablesConfig.Collectables)
            {
                _typeToPrefab.Add(container.Type, container.Prefab);
            }
            
            _localMessageBroker.Subscribe<LevelPartPlacedMessage>(this);
            _localMessageBroker.Subscribe<LevelPartWillBeRemovedMessage>(this);
            _localMessageBroker.Subscribe<CatchCollectableMessage>(this);
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

        public void OnMessage(in LevelPartPlacedMessage message)
        {
            if (message.Value.SpawnedCollectables != null)
            {
                Debug.LogError("[CollectablesController] Didn't cleaned spawned collectables for level part");
                CleanupLevelPart(message.Value);
            }
            
            if (message.Value.CollectablePlaces == null || message.Value.CollectablePlaces.Length == 0)
                return;

            message.Value.SpawnedCollectables = ListPool<BaseCollectable>.Get();
            foreach (var place in message.Value.CollectablePlaces)
            {
                if (!_typeToPrefab.TryGetValue(place.Type, out var prefab))
                {
                    Debug.LogError($"[CollectablesController] Can't find prefab for type {place.Type.ToString()}");
                    continue;
                }

                var instance = GameContainer.Current.InstantiateAndResolve(prefab);
                instance.transform.SetParent(place.transform);
                instance.transform.MoveToLocalZero();
                message.Value.SpawnedCollectables.Add(instance);
            }
        }

        public void OnMessage(in LevelPartWillBeRemovedMessage message)
        {
            if (message.Value.SpawnedCollectables != null)
                CleanupLevelPart(message.Value);
        }

        public void OnMessage(in CatchCollectableMessage message)
        {
            var type = message.Value.GetType();
            if (!_typeToHandler.TryGetValue(type, out var handler))
            {
                Debug.LogError($"[CollectablesController] No handler registered for type {type.Name}");
                return;
            }
            
            handler.HandleCollectable(message.Value);
            message.Value.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _localMessageBroker.Unsubscribe<LevelPartPlacedMessage>(this);
            _localMessageBroker.Unsubscribe<LevelPartWillBeRemovedMessage>(this);
            _localMessageBroker.Unsubscribe<CatchCollectableMessage>(this);
        }

        private void CleanupLevelPart(LevelPart levelPart)
        {
            if (levelPart.SpawnedCollectables == null)
                return;

            foreach (var spawnedCollectable in levelPart.SpawnedCollectables)
            {
                PrefabMonoPool<BaseCollectable>.ReturnInstance(spawnedCollectable);
            }
            
            levelPart.SpawnedCollectables.Clear();
            ListPool<BaseCollectable>.Release(levelPart.SpawnedCollectables);
            levelPart.SpawnedCollectables = null;
        }
    }
}