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
        [SerializeField] private GameObject _playerShadowPrefab;
        
        public override UniTask Initialize()
        {
            var inputState = GameContainer.Current.Create<InputState>();
            inputState.Initialize();
            GameContainer.Current.Register(inputState);

#if UNITY_EDITOR
            var desktopInputReader = GameContainer.Current.Create<DesktopInputSource>();
            desktopInputReader.Initialize();
            GameContainer.Current.AddDisposable(desktopInputReader);
#endif
            
            var visuals = PrefabMonoPool<CharacterVisuals>.GetPrefabInstance(_characterVisualsPrefab);
            
            var character = GameContainer.Current.InstantiateAndResolve(_playerCharacterPrefab);
            character.Initialize(visuals);
            GameContainer.Current.Register(character);

            var shadow = PrefabGameObjectPool.GetPrefabInstance(_playerShadowPrefab);
            
            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}