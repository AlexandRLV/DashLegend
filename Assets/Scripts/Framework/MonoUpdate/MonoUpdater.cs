using System.Collections.Generic;
using UnityEngine;

namespace Framework.MonoUpdate
{
    public class MonoUpdater : MonoBehaviour
    {
        private List<IUpdatable> _updatables;
        private List<IFixedUpdatable> _fixedUpdatables;
        private List<ILateUpdatable> _lateUpdatables;
        private List<IOneSecondUpdatable> _oneSecondUpdatables;

        private List<object> _updatablesToAdd;
        private List<object> _updatablesToRemove;

        private float _oneSecondUpdateTimer;

        private void Awake()
        {
            _updatables = new List<IUpdatable>();
            _fixedUpdatables = new List<IFixedUpdatable>();
            _lateUpdatables = new List<ILateUpdatable>();
            _oneSecondUpdatables = new List<IOneSecondUpdatable>();
            _updatablesToAdd = new List<object>();
            _updatablesToRemove = new List<object>();
        }

        private void Update()
        {
            foreach (var updatable in _updatablesToRemove)
            {
                RemoveUpdatableInternal(updatable);
            }
            _updatablesToRemove.Clear();

            foreach (var updatable in _updatablesToAdd)
            {
                AddUpdatableInternal(updatable);
            }
            _updatablesToAdd.Clear();
            
            for (int i = 0; i < _updatables.Count; i++)
            {
                var updatable = _updatables[i];
                if (updatable == null)
                {
                    _updatables.RemoveAt(i);
                    i--;
                    continue;
                }
                
                updatable.Update();
            }

            _oneSecondUpdateTimer += Time.deltaTime;
            if (_oneSecondUpdateTimer < 1f) return;
            
            _oneSecondUpdateTimer -= 1f;
            for (int i = 0; i < _oneSecondUpdatables.Count; i++)
            {
                var updatable = _oneSecondUpdatables[i];
                if (updatable == null)
                {
                    _oneSecondUpdatables.RemoveAt(i);
                    i--;
                    continue;
                }
                
                updatable.OneSecondUpdate();
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _fixedUpdatables.Count; i++)
            {
                var updatable = _fixedUpdatables[i];
                if (updatable == null)
                {
                    _fixedUpdatables.RemoveAt(i);
                    i--;
                    continue;
                }
                
                updatable.FixedUpdate();
            }
        }

        private void LateUpdate()
        {
            for (int i = 0; i < _lateUpdatables.Count; i++)
            {
                var updatable = _lateUpdatables[i];
                if (updatable == null)
                {
                    _lateUpdatables.RemoveAt(i);
                    i--;
                    continue;
                }
                
                updatable.LateUpdate();
            }
        }

        public void AddUpdatable(object target)
        {
            _updatablesToAdd.Add(target);
        }

        public void RemoveUpdatable(object target)
        {
            _updatablesToRemove.Add(target);
        }

        private void AddUpdatableInternal(object target)
        {
            if (target is IUpdatable updatable)
                _updatables.Add(updatable);
            
            if (target is IFixedUpdatable fixedUpdatable)
                _fixedUpdatables.Add(fixedUpdatable);
            
            if (target is ILateUpdatable lateUpdatable)
                _lateUpdatables.Add(lateUpdatable);
            
            if (target is IOneSecondUpdatable oneSecondUpdatable)
                _oneSecondUpdatables.Add(oneSecondUpdatable);
        }

        private void RemoveUpdatableInternal(object target)
        {
            if (target is IUpdatable updatable)
                _updatables.Remove(updatable);
            
            if (target is IFixedUpdatable fixedUpdatable)
                _fixedUpdatables.Remove(fixedUpdatable);
            
            if (target is ILateUpdatable lateUpdatable)
                _lateUpdatables.Remove(lateUpdatable);
            
            if (target is IOneSecondUpdatable oneSecondUpdatable)
                _oneSecondUpdatables.Remove(oneSecondUpdatable);
        }
    }
}