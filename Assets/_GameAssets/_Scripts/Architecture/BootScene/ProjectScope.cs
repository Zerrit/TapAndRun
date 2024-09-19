using TapAndRun.Services.Ads;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TapAndRun.Architecture.BootScene
{
    public class ProjectScope : LifetimeScope
    {
        [Header("Services")]
        [SerializeField] private AdsService _adsService;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_adsService).AsImplementedInterfaces();

            builder.Register<BootEntryPoint>(Lifetime.Singleton).As<IAsyncStartable>();
        }
    }
}