using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.Initialization;
using Framework.Pools;
using GameCore.Character;
using GameCore.Input;
using UnityEngine;

namespace Startup.LevelInitializers
{
    public class PlayerInitializer : InitializerBase
    {
        [SerializeField] private PlayerCharacter _playerCharacterPrefab;
        [SerializeField] private CharacterVisuals _characterVisualsPrefab;
        
        public override UniTask Initialize()
        {
            GameContainer.Current.Register(new InputState());
            
            var visuals = PrefabMonoPool<CharacterVisuals>.GetPrefabInstance(_characterVisualsPrefab);
            
            var character = GameContainer.Current.InstantiateAndResolve(_playerCharacterPrefab);
            character.Initialize(visuals);
            GameContainer.Current.Register(character);
            
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}