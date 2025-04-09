using System.Collections.Generic;
using Framework.Extensions;
using UnityEngine;

namespace Framework.Pools
{
    public class PrefabMonoPool<T> where T : Component
	{
		// ReSharper disable once StaticMemberInGenericType
		private static readonly Dictionary<int, int> _spawnedInstancesPrefabIds = new();
		private static readonly Dictionary<int, PrefabMonoPool<T>> _prefabPools = new();

		// ReSharper disable once StaticMemberInGenericType
		private static GameObject _mainRoot;
		
		private GameObject _root;
		private readonly T _prefab;
		private readonly Queue<T> _queue;

		private PrefabMonoPool(T prefab, int initialCapacity)
		{
			var type = typeof(T);
			int instanceId = prefab.GetInstanceID();
			_root = new GameObject($"Prefab pool {type.Name} - {instanceId.ToString()}");
			_root.transform.SetParent(_mainRoot.transform);
			_prefab = prefab;
			_queue = new Queue<T>();

			for (int i = 0; i < initialCapacity; i++)
			{
				var instance = Object.Instantiate(prefab, _root.transform);
				instance.gameObject.SetActive(false);
				_queue.Enqueue(instance);
			}
		}

		private void EnsureCreatedRoot()
		{
			if (_root != null) return;
			
			var type = typeof(T);
			_root = new GameObject($"Prefab pool {type.Name} - {_prefab.GetInstanceID().ToString()}");
			_root.transform.SetParent(_mainRoot.transform);
			_queue.Clear();
		}

		private T Get()
		{
			EnsureCreatedRoot();
			
			var instance = _queue.TryDequeue(out var result) ? result : Object.Instantiate(_prefab, _root.transform);
			if (instance == null)
			{
				Debug.LogError($">>> Requesting pooled instance of type {typeof(T).Name} NULL!");
				return null;
			}
			
			instance.transform.SetParent(null);
			instance.gameObject.SetActive(true);
			return instance;
		}

		private void Return(T value)
		{
			if (value == null) return;
			if (_root == null) return;
			
			value.gameObject.SetActive(false);
			value.transform.SetParent(_root.transform);
			_queue.Enqueue(value);
		}

		public static T GetPrefabInstance(T prefab, int initialCapacity = 0)
		{
			EnsureCreatedMainRoot();
			
			int prefabInstanceId = prefab.GetInstanceID();
			if (!_prefabPools.TryGetValue(prefabInstanceId, out var pool))
			{
				pool = new PrefabMonoPool<T>(prefab, initialCapacity);
				_prefabPools.Add(prefabInstanceId, pool);
			}
			
			var instance = pool.Get();
			_spawnedInstancesPrefabIds.Add(instance.GetInstanceID(), prefabInstanceId);
			return instance;
		}

		public static T GetPrefabInstanceForParent(T prefab, Transform parent, int initialCapacity = 0)
		{
			var instance = GetPrefabInstance(prefab, initialCapacity);
			instance.transform.SetParent(parent);
			instance.transform.MoveToLocalZero();
			
			return instance;
		}
		
		public static void ReturnInstance(T instance)
		{
			if (instance == null)
				return;
			
			int instanceId = instance.GetInstanceID();
			if (!_spawnedInstancesPrefabIds.TryGetValue(instanceId, out int prefabId))
			{
#if UNITY_EDITOR
				if (!Application.isPlaying)
				{
					Object.DestroyImmediate(instance.gameObject);
					return;
				}
#endif
				
				Object.Destroy(instance.gameObject);
				return;
			}

			var pool = _prefabPools[prefabId];
			pool.Return(instance);
			_spawnedInstancesPrefabIds.Remove(instance.GetInstanceID());
		}

		private static void EnsureCreatedMainRoot()
		{
			if (_mainRoot != null) return;

			_mainRoot = new GameObject("Prefab Game Objects Pools");
			_prefabPools.Clear();
			_spawnedInstancesPrefabIds.Clear();
		}
    }
}