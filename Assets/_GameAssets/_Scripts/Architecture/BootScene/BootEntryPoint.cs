using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Architecture.GameStates;
using TapAndRun.Interfaces;
using TapAndRun.Services.Ads;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace TapAndRun.Architecture.BootScene
{
    public class BootEntryPoint : IAsyncStartable
    {
        private List<IInitializableAsync> _initializationQueue;

        [Inject]
        public void Construct(IAdsService adsService)
        {
            _initializationQueue = new List<IInitializableAsync>
            {
                adsService
            };
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            Application.targetFrameRate = 60;

            foreach (var entity in _initializationQueue)
            {
                await entity.InitializeAsync(cancellation);
            }

            SceneManager.LoadScene(1);
        }
    }
}