﻿using TapAndRun.Architecture.GameStates;
using TapAndRun.Configs;
using TapAndRun.Factories.GameStates;
using TapAndRun.Factories.LevelButtons;
using TapAndRun.Factories.Levels;
using TapAndRun.MVP.Character;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Character.View;
using TapAndRun.MVP.CharacterCamera;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.Presenter;
using TapAndRun.MVP.Levels.Views;
using TapAndRun.MVP.Lose;
using TapAndRun.MVP.Lose.Model;
using TapAndRun.MVP.Lose.View;
using TapAndRun.MVP.MainMenu;
using TapAndRun.MVP.MainMenu.Model;
using TapAndRun.MVP.MainMenu.Views;
using TapAndRun.MVP.Screens.LevelSelect;
using TapAndRun.MVP.Settings;
using TapAndRun.MVP.Settings.Model;
using TapAndRun.MVP.Settings.Views;
using TapAndRun.MVP.Wallet;
using TapAndRun.MVP.Wallet.Model;
using TapAndRun.MVP.Wallet.View;
using TapAndRun.Services.Audio;
using TapAndRun.Services.Localization;
using TapAndRun.Services.Update;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace TapAndRun.Architecture.Installers
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
        [SerializeField] private SoundConfig _soundConfig;
        [SerializeField] private CameraConfig _cameraConfig;
        [SerializeField] private LevelsConfig _levelsConfig;
        [SerializeField] private CharacterConfig _characterConfig;

        [Header("Services")]
        [SerializeField] private UpdateService _updateService;
        [SerializeField] private LocalisationService _localisationService;
        [SerializeField] private AudioService _audioService;

        [Header("Views")]
        [SerializeField] private CharacterView _characterView;
        [SerializeField] private CameraView _cameraView;
        [SerializeField] private WalletView _walletView;
        [SerializeField] private MainMenuView _mainMenu;
        [SerializeField] private SettingView _settingsView;
        [SerializeField] private LevelSelectView _levelSelectView;
        [SerializeField] private GameplayScreenView _gameplayScreenView;
        [SerializeField] private LoseView _loseView;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterFactories(builder);
            RegisterServices(builder);

            RegisterCamera(builder);
            RegisterCharacter(builder);
            RegisterLevels(builder);
            RegisterWallet(builder);
            RegisterSettings(builder);
            RegisterLoseScreen(builder);
            RegisterMainMenu(builder);

            RegisterGameStates(builder);

            builder.Register<GameStateMachine>(Lifetime.Singleton);
            builder.RegisterInstance(_entryPoint).As<IAsyncStartable>();
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<GameStateFactory>(Lifetime.Singleton).As<IGameStateFactory>();
            
            builder.Register<LevelFactory>(Lifetime.Singleton).As<ILevelFactory>()
                .WithParameter(_levelsConfig)
                .WithParameter(_levelsParent);

            builder.Register<LevelButtonFactory>(Lifetime.Singleton).As<ILevelButtonFactory>()
                .WithParameter(_levelButtonPrefab);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.RegisterInstance(_updateService).AsImplementedInterfaces();

            builder.RegisterInstance(_audioService).AsImplementedInterfaces();
        }

        private void RegisterCamera(IContainerBuilder builder)
        {
            builder.Register<CameraModel>(Lifetime.Singleton).AsImplementedInterfaces()
                .WithParameter(_cameraConfig);
            builder.Register<CameraPresenter>(Lifetime.Singleton)
                .WithParameter(_cameraView);
        }
        
        private void RegisterCharacter(IContainerBuilder builder)
        {
            builder.Register<CharacterModel>(Lifetime.Singleton).AsImplementedInterfaces()
                .WithParameter(_characterConfig);
            builder.Register<CharacterPresenter>(Lifetime.Singleton)
                .WithParameter(_characterView);
        }

        private void RegisterLevels(IContainerBuilder builder)
        {
            builder.Register<LevelsModel>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LevelsPresenter>(Lifetime.Singleton)
                .WithParameter(_gameplayScreenView);
        }

        private void RegisterWallet(IContainerBuilder builder)
        {
            builder.Register<WalletModel>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WalletPresenter>(Lifetime.Singleton)
                .WithParameter( _walletView);
        }

        private void RegisterSettings(IContainerBuilder builder)
        {
            builder.Register<SettingsModel>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SettingsPresenter>(Lifetime.Singleton)
                .WithParameter(_settingsView);
        }

        private void RegisterLoseScreen(IContainerBuilder builder)
        {
            builder.Register<LoseModel>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LosePresenter>(Lifetime.Singleton)
                .WithParameter(_loseView);
        }
        
        private void RegisterMainMenu(IContainerBuilder builder)
        {
            builder.Register<SelfMainMenuModel>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<MainMenuPresenter>(Lifetime.Singleton)
                .WithParameter(_mainMenu)
                .WithParameter(_levelSelectView);
        }
        
        private void RegisterGameStates(IContainerBuilder builder)
        {
            builder.Register<MainMenuState>(Lifetime.Singleton);
            builder.Register<GameplayState>(Lifetime.Singleton);
            builder.Register<LoseState>(Lifetime.Singleton);
        }
    }
}