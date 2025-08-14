using Currency;
using Framework;
using Framework.GameStateMachine;
using Framework.GUI;
using Framework.Sounds;
using GameCore;
using GameCore.Boosters;
using GameCore.Boosters.Handlers;
using GameCore.Character;
using GameCore.Collectables;
using GameCore.Collectables.HandlerTypes;
using GameCore.CommonShadow;
using GameCore.Input;
using GameCore.Level;
using GameCore.Level.Props;
using GameCore.Skins;
using GUI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Startup
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private ScriptableConfigRoot _configRoot;
        [SerializeField] private UiRoot _uiRoot;
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private SoundSystem _soundSystemPrefab;
        [SerializeField] private LevelGenerator _levelGeneratorPrefab;
        
        protected override void Configure(IContainerBuilder builder)
        {
            foreach (var scriptableConfig in _configRoot.Configs)
            {
                builder.RegisterInstance(scriptableConfig, scriptableConfig.GetType());
            }
            
            builder.RegisterInstance(_uiRoot);
            builder.RegisterInstance(_loadingScreen);
            
            builder.Register<SceneLoader>(Lifetime.Singleton);
            builder.Register<LocalMessageBroker>(Lifetime.Singleton);
            builder.Register<GameTime>(Lifetime.Singleton);
            builder.Register<WindowsSystem>(Lifetime.Singleton);
            builder.Register<GameStateMachine>(Lifetime.Singleton);
            builder.Register<CollectablesSpawner>(Lifetime.Singleton);
            builder.Register<LevelPropsSpawner>(Lifetime.Singleton);
            builder.Register<PlayerSpawnService>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<PlayerCurrencyController>().AsSelf();
            builder.RegisterEntryPoint<SettingsProvider>().AsSelf();
            builder.RegisterEntryPoint<ShadowController>().AsSelf();
            builder.RegisterEntryPoint<GameController>().AsSelf();
            builder.RegisterEntryPoint<CharacterSkinService>().AsSelf();
            
            builder.RegisterEntryPoint<BoostersService>().AsSelf();
            builder.RegisterEntryPoint<MagnetBoosterHandler>();
            builder.RegisterEntryPoint<ShieldBoosterHandler>();
            builder.RegisterEntryPoint<JetPackBoosterHandler>();
            
            builder.RegisterEntryPoint<CurrencyCollectableHandler>();
            builder.RegisterEntryPoint<BoosterCollectableHandler>();
            builder.RegisterEntryPoint<InputState>().AsSelf();
            
#if UNITY_EDITOR
            builder.RegisterEntryPoint<DesktopInputSource>();
#endif

            builder.RegisterComponentInNewPrefab(_soundSystemPrefab, Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab(_levelGeneratorPrefab, Lifetime.Singleton);
        }
    }
}