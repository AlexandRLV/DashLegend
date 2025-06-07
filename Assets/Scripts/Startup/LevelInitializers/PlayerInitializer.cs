using Cysharp.Threading.Tasks;
using Framework.DI;
using Framework.Initialization;
using Framework.Pools;
using GameCore.Character;
using GameCore.Input;
using GameCore.Skins;
using UnityEngine;

namespace Startup.LevelInitializers
{
    public class PlayerInitializer : InitializerBase
    {
        [SerializeField] private float _groundYOffset;
        [SerializeField] private PlayerCharacter _playerCharacterPrefab;
        [SerializeField] private CharacterVisuals _characterVisualsPrefab;

        [Inject] private readonly CharacterSkinService _characterSkinService;
        
        public override UniTask Initialize()
        {
            GameContainer.Current.CreateAndRegister<InputState>();

#if UNITY_EDITOR
            GameContainer.Current.CreateAndRegister<DesktopInputSource>();
#endif
            
            var visuals = PrefabMonoPool<CharacterVisuals>.GetPrefabInstance(_characterVisualsPrefab);
            visuals.SetSkin(_characterSkinService.SelectedSkin);
            
            var character = GameContainer.Current.InstantiateAndRegister(_playerCharacterPrefab);
            character.transform.position = Vector3.up * _groundYOffset;
            character.Initialize(visuals, _groundYOffset);

            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
        }
    }
}