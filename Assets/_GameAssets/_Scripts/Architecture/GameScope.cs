using TapAndRun.CameraLogic;
using TapAndRun.Configs;
using TapAndRun.Factories.Levels;
using TapAndRun.MVP.Character.View;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.Presenter;
using TapAndRun.MVP.Screens.Level;
using TapAndRun.MVP.Screens.Lose;
using TapAndRun.MVP.Screens.Main;
using UnityEngine;
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
        
        [Header("Configs")]
        [SerializeField] private LevelsConfig _levelsConfig;

        [Header("Views")]
        [SerializeField] private CharacterView _characterView;
        [SerializeField] private CharacterCamera _camera;
        [SerializeField] private MainScreenView _mainScreen;
        [SerializeField] private LevelScreenView _levelScreen;
        [SerializeField] private LoseScreenView _loseScreen;
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfigs(builder);
            RegisterFactories(builder);

            RegisterLevels(builder);
            RegisterMainScreen(builder);

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
        }

        private void RegisterLevels(IContainerBuilder builder)
        {
            builder.Register<LevelsModel>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LevelsPresenter>(Lifetime.Singleton)
                .WithParameter(_characterView)
                .WithParameter(_camera)
                .WithParameter(_levelScreen)
                .WithParameter(_loseScreen);
        }
        
        private void RegisterMainScreen(IContainerBuilder builder)
        {
            builder.Register<MainScreenModel>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<MainScreenPresenter>(Lifetime.Singleton)
                .WithParameter(_mainScreen);
        }
    }
}