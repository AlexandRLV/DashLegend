using System;
using System.Collections.Generic;
using Framework;
using GameCore.Boosters.Handlers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameCore.Boosters
{
    public class BoostersService : IInitializable, ITickable, IDisposable
    {
        public event Action<ActiveBoosterContainer> OnBoosterActivated;
        public event Action<ActiveBoosterContainer> OnBoosterDeactivated;
        
        public ReadOnlyList<ActiveBoosterContainer> ActiveBoosters => _activeHandlers;
        
        public class ActiveBoosterContainer
        {
            public BoosterType Type;
            public bool Active;
            public float ActiveTime;
            public BoosterHandlerBase Handler;
        }
        
        [Inject] private readonly GameTime _gameTime;

        private List<ActiveBoosterContainer> _activeHandlers;
        private List<ActiveBoosterContainer> _boostersToDeactivate;
        private Dictionary<BoosterType, ActiveBoosterContainer> _boosterHandlers;
        
        public void Initialize()
        {
            _activeHandlers = new List<ActiveBoosterContainer>();
            _boostersToDeactivate = new List<ActiveBoosterContainer>();
            _boosterHandlers = new Dictionary<BoosterType, ActiveBoosterContainer>();
        }

        public bool IsBoosterActive(BoosterType type) =>
            _boosterHandlers.TryGetValue(type, out var container) && container.Active;
        
        public void RegisterBoosterHandler(BoosterType type, BoosterHandlerBase handler)
        {
            if (_boosterHandlers.ContainsKey(type))
            {
                Debug.LogError($"[BoostersService] Duplicate registration of booster handler for {type}");
                return;
            }

            _boosterHandlers[type] = new ActiveBoosterContainer
            {
                Type = type,
                Active = false,
                ActiveTime = 0f,
                Handler = handler
            };
        }

        public void UnregisterHandler(BoosterType type, BoosterHandlerBase handler)
        {
            if (!_boosterHandlers.TryGetValue(type, out var container))
            {
                Debug.LogError($"[BoostersService] Trying to unregister not registered booster handler for {type}");
                return;
            }

            if (container.Handler != handler)
            {
                Debug.LogError($"[BoostersService] Trying to unregister handler, bur registered different handler for {type}");
                return;
            }

            if (container.Active)
                container.Handler.Disable();

            _activeHandlers.Remove(container);
            _boosterHandlers.Remove(type);
        }
        
        public void ActivateBooster(BoosterType type, float duration)
        {
            if (!_boosterHandlers.TryGetValue(type, out var container))
            {
                Debug.LogError($"[BoostersService] Can't get booster handler for type {type}");
                return;
            }
            
            if (container.ActiveTime <= 0f)
                container.ActiveTime = duration;
            else
                container.ActiveTime += duration;

            if (!container.Active)
            {
                container.Handler.Enable();
                container.Active = true;
            }
            
            if (!_activeHandlers.Contains(container))
                _activeHandlers.Add(container);
            
            OnBoosterActivated?.Invoke(container);
        }

        public void DeactivateBooster(BoosterType type)
        {
            if (!_boosterHandlers.TryGetValue(type, out var container))
            {
                Debug.LogError($"[BoostersService] Can't get booster handler for type {type}");
                return;
            }

            DeactivateBooster(container);
        }

        private void DeactivateBooster(ActiveBoosterContainer container)
        {
            if (container.Active)
            {
                container.Active = false;
                container.Handler.Disable();
            }

            container.ActiveTime = 0f;
            _activeHandlers.Remove(container);
            OnBoosterDeactivated?.Invoke(container);
        }

        public void Tick()
        {
            foreach (var container in _activeHandlers)
            {
                container.Handler.Update();
                container.ActiveTime -= _gameTime.DeltaTime;
                if (container.ActiveTime <= 0f)
                    _boostersToDeactivate.Add(container);
            }

            foreach (var container in _boostersToDeactivate)
            {
                DeactivateBooster(container);
            }
            _boostersToDeactivate.Clear();
        }

        public void Dispose()
        {
            foreach (var container in _activeHandlers)
            {
                DeactivateBooster(container);
            }
        }
    }
}