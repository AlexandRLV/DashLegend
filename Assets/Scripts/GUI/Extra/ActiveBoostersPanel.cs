using System.Collections.Generic;
using Framework.Extensions;
using Framework.Pools;
using GameCore.Boosters;
using UnityEngine;
using VContainer;

namespace GUI
{
    public class ActiveBoostersPanel : MonoBehaviour
    {
        [SerializeField] private Transform _activeBoostersItemsParent;
        [SerializeField] private ActiveBoosterItem _activeBoosterItemPrefab;
        
        [Inject] private readonly BoostersService _boostersService;
        [Inject] private readonly BoostersConfig _boostersConfig;
        
        private Dictionary<BoosterType, ActiveBoosterItem> _activeBoosters;

        private void Start()
        {
            _activeBoosters = new Dictionary<BoosterType, ActiveBoosterItem>();
            _boostersService.OnBoosterActivated += OnBoosterActivated;
            _boostersService.OnBoosterDeactivated += OnBoosterDeactivated;
        }

        private void OnDestroy()
        {
            _boostersService.OnBoosterActivated -= OnBoosterActivated;
            _boostersService.OnBoosterDeactivated -= OnBoosterDeactivated;
        }

        private void OnBoosterActivated(BoostersService.ActiveBoosterContainer container)
        {
            if (_activeBoosters.TryGetValue(container.Type, out var item))
            {
                item.SetTime(container.ActiveTime);
                return;
            }
            
            if (!_boostersConfig.SpriteContainers.TryGetValueByEnumKey(container.Type, out Sprite sprite))
                return;
            
            item = PrefabMonoPool<ActiveBoosterItem>.GetPrefabInstance(_activeBoosterItemPrefab, _activeBoostersItemsParent);
            item.Init(sprite, container.ActiveTime);
            _activeBoosters.Add(container.Type, item);
        }

        private void OnBoosterDeactivated(BoostersService.ActiveBoosterContainer container)
        {
            if (!_activeBoosters.TryGetValue(container.Type, out var item))
                return;
            
            PrefabMonoPool<ActiveBoosterItem>.ReturnInstance(item);
            _activeBoosters.Remove(container.Type);
        }
    }
}