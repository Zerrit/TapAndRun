using TapAndRun.CameraLogic;
using TapAndRun.Configs;
using TapAndRun.Factories.LevelButtons;
using TapAndRun.Factories.Levels;
using TapAndRun.MVP.Character.View;
using TapAndRun.MVP.Gameplay.Model;
using TapAndRun.MVP.Gameplay.Presenter;
using TapAndRun.MVP.Gameplay.Views;
using TapAndRun.MVP.MainMenu;
using TapAndRun.MVP.MainMenu.Model;
using TapAndRun.MVP.MainMenu.Views;
using TapAndRun.MVP.Screens.LevelSelect;
using TapAndRun.MVP.Settings;
using TapAndRun.MVP.Settings.Model;
using TapAndRun.MVP.Settings.Views;
using TapAndRun.MVP.Wallet;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace TapAndRun.Architecture
{
    public class GameScope : LifetimeScope
    {
        [Header("Entry Point")]
        [SerializeField] private EntryPoint _entryPoint;
        
        [Header("Objects")]
        [SerializeField] private Transform _levelsParent;
        
        [Header("Prefabs")]
        [SerializeField] private AssetReference _levelButtonPrefab;
        
        [Header("Configs")]
        [SerializeField] private LevelsConfig _levelsConfig;

        [Header("Views")]
        [SerializeField] private CharacterView _characterView;
        [SerializeField] private CharacterCamera _camera;
        [SerializeField] private WalletView _wallet;
        [SerializeField] private MainScreenView _mainScreen;
        [SerializeField] private SettingView _settingsView;
        [SerializeField] private LevelSelectView _levelSelectPopup;
        [SerializeField] private GameplayScreenView _gameplayScreen;
        [SerializeField] private LoseScreenView _loseScreen;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfigs(builder);
            RegisterFactories(builder);

            RegisterLevels(builder);
            RegisterSettings(builder);
            RegisterMainMenu(builder);

            builder.RegisterInstance(_entryPoint).As<IAsyncStartable>();
        }

        private void RegisterConfigs(IContainerBuilder builder)
        {

        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<LevelFactory>(Lifetime.Singleton).As<ILevelFactory>()
                .WithParameter(_levelsConfig)
                .WithParameter(_levelsParent);

            builder.Register<LevelButtonFactory>(Lifetime.Singleton).As<ILevelButtonFactory>()
                .WithParameter(_levelButtonPrefab);
        }

        private void RegisterLevels(IContainerBuilder builder)
        {
            builder.Register<GameplayModel>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LevelsPresenter>(Lifetime.Singleton)
                .WithParameter(_characterView)
                .WithParameter(_camera)
                .WithParameter(_gameplayScreen)
                .WithParameter(_loseScreen)
                .WithParameter(_wallet);
        }
        
        private void RegisterSettings(IContainerBuilder builder)
        {
            builder.Register<SettingsModel>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SettingsPresenter>(Lifetime.Singleton)
                .WithParameter(_settingsView);
        }
        
        private void RegisterMainMenu(IContainerBuilder builder)
        {
            builder.Register<MainScreenModel>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<MainScreenPresenter>(Lifetime.Singleton)
                .WithParameter(_mainScreen);
        }
    }
}