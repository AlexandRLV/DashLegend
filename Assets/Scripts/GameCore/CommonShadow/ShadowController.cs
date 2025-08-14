using System.Collections.Generic;
using Framework.Extensions;
using Framework.Pools;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameCore.CommonShadow
{
    public class ShadowController : ITickable
    {
        [Inject] private readonly ShadowConfig _shadowConfig;
        [Inject] private readonly IObjectResolver _container;

        private readonly List<(Transform, Transform)> _objectsWithShadow = new();

        public void Tick()
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
            _container.Inject(shadow);
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