using System;
using System.Collections.Generic;
using Framework.DI;
using Framework.Extensions;
using Framework.MonoUpdate;
using Framework.Pools;
using UnityEngine;

namespace GameCore.CommonShadow
{
    public class ShadowController : IInitializable, IUpdatable, IDisposable
    {
        [Inject] private readonly MonoUpdater _monoUpdater;
        [Inject] private readonly ShadowConfig _shadowConfig;

        private List<(Transform, Transform)> _objectsWithShadow = new();

        public void Initialize() => _monoUpdater.AddUpdatable(this);
        public void Dispose() => _monoUpdater.RemoveUpdatable(this);

        public void Update()
        {
            foreach (var (source, shadow) in _objectsWithShadow)
            {
                shadow.transform.position = source.transform.position.WithY(_shadowConfig.FloorHeightOffset);
            }
        }

        public void AddShadow(Transform source, float scale)
        {
            if (scale <= 0f) return;
            
            var shadow = PrefabGameObjectPool.GetPrefabInstance(_shadowConfig.ShadowPrefab);
            shadow.transform.position = source.transform.position.WithY(_shadowConfig.FloorHeightOffset);
            shadow.transform.localScale = Vector3.one * scale;
            _objectsWithShadow.Add((source, shadow.transform));
        }

        public void RemoveShadow(Transform source)
        {
            for (int i = 0; i < _objectsWithShadow.Count; i++)
            {
                var container = _objectsWithShadow[i];
                if (container.Item1 != source)
                    continue;
                
                if (container.Item2 != null)
                    PrefabGameObjectPool.ReturnInstance(container.Item2.gameObject);
                    
                _objectsWithShadow.RemoveAt(i);
                return;
            }
        }
    }
}