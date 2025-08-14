using GameCore.Character;
using GameCore.Collectables;
using UnityEngine;
using VContainer;

namespace GameCore.Boosters.Handlers
{
    public class MagnetBoosterHandler : BoosterHandlerBase
    {
        protected override BoosterType Type => BoosterType.Magnet;

        [Inject] private readonly BoostersConfig _boostersConfig;
        [Inject] private readonly PlayerSpawnService _playerSpawnService;
        [Inject] private readonly CollectablesSpawner _collectablesSpawner;

        protected override void InitializeInternal()
        {
        }

        public override void Enable()
        {
            // TODO: enable character magnet effect
        }

        public override void Update()
        {
            var playerPosition = _playerSpawnService.Character.transform.position;
            foreach (var collectable in _collectablesSpawner.ActiveCollectables)
            {
                var position = collectable.transform.position;
                float distance = Vector3.Distance(playerPosition, position);
                if (distance >= _boostersConfig.MagnetRadius) continue;
                
                collectable.MoveToMagnet(_playerSpawnService.Character.transform, _boostersConfig.MagnetSpeed);
            }
        }

        public override void Disable()
        {
            // TODO: disable character magnet effect
        }
    }
}