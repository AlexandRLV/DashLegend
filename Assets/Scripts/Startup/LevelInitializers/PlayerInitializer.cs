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
        [SerializeField] private float _groundYOffset;
        [SerializeField] private PlayerCharacter _playerCharacterPrefab;
        [SerializeField] private CharacterVisuals _characterVisualsPrefab;
        [SerializeField] private GameObject _playerShadowPrefab;
        
        public override UniTask Initialize()
        {
            GameContainer.Current.CreateAndRegister<InputState>();

#if UNITY_EDITOR
            GameContainer.Current.CreateAndRegister<DesktopInputSource>();
#endif
            
            var visuals = PrefabMonoPool<CharacterVisuals>.GetPrefabInstance(_characterVisualsPrefab);
            
            var character = GameContainer.Current.InstantiateAndResolve(_playerCharacterPrefab);
            character.transform.position = Vector3.up * _groundYOffset;
            character.Initialize(visuals, _groundYOffset);
            GameContainer.Current.Register(character);

            var shadow = PrefabGameObjectPool.GetPrefabInstance(_playerShadowPrefab);
            shadow.transform.position = Vector3.up * _groundYOffset;
            
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}