using Framework.Pools;
using GameCore.Skins;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameCore.Character
{
    public class PlayerSpawnService
    {
        public PlayerCharacter Character => _spawnedCharacter;

        [Inject] private readonly IObjectResolver _container;
        [Inject] private readonly CharacterSkinService _characterSkinService;
        [Inject] private readonly GameConfig _gameConfig;

        private PlayerCharacter _spawnedCharacter;
        private CharacterVisuals _spawnedVisuals;

        public void Spawn()
        {
            _spawnedVisuals = PrefabMonoPool<CharacterVisuals>.GetPrefabInstance(_gameConfig.CharacterVisualsPrefab);
            _container.InjectGameObject(_spawnedVisuals.gameObject);
            _spawnedVisuals.SetSkin(_characterSkinService.SelectedSkin);
            
            _spawnedCharacter = _container.Instantiate(_gameConfig.PlayerCharacterPrefab);
            _spawnedCharacter.transform.position = Vector3.up * _gameConfig.GroundYOffset;
            _spawnedCharacter.Initialize(_spawnedVisuals, _gameConfig.GroundYOffset);
        }
    }
}