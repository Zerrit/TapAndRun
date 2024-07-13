using TapAndRun.Configs;
using TapAndRun.Factories;
using TapAndRun.MVP.Character.Model;
using TapAndRun.MVP.Character.Presenter;
using TapAndRun.MVP.Character.View;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.Presenter;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TapAndRun.Architecture
{
    public class GameScope : LifetimeScope
    {
        [Header("Configs")]
        [SerializeField] private LevelsConfig _levelsConfig;

        [Header("Views")]
        [SerializeField] private CharacterView _view;
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfigs(builder);
            RegisterFactories(builder);

            RegisterCharacter(builder);
            RegisterLevels(builder);
        }

        private void RegisterConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_levelsConfig);
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<LevelFactory>(Lifetime.Singleton).As<ILevelFactory>();
        }

        private void RegisterCharacter(IContainerBuilder builder)
        {
            builder.Register<CharacterModel>(Lifetime.Singleton);
            builder.RegisterComponent(_view);
            builder.Register<CharacterPresenter>(Lifetime.Singleton);
        }
        
        private void RegisterLevels(IContainerBuilder builder)
        {
            builder.Register<LevelsModel>(Lifetime.Singleton);
            builder.Register<LevelsPresenter>(Lifetime.Singleton);
        }
    }
}