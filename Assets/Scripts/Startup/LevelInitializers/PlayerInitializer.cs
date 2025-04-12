using System.Collections.Generic;
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
            var desktopInputReader = GameContainer.Current.Create<DesktopInputReader>();
            desktopInputReader.Initialize();
            // TODO: touch input reader
            GameContainer.Current.AddDisposable(desktopInputReader);

            var inputState = GameContainer.Current.Create<InputState>();
            inputState.Initialize(new List<IInputSource>
            {
                desktopInputReader,
            });
            GameContainer.Current.Register(inputState);
            
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