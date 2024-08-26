using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Screens.LevelSelect;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TapAndRun.Factories.LevelButtons
{
    public class LevelButtonFactory : ILevelButtonFactory, IDisposable
    {
        private readonly AssetReference _levelButtonPrefab;

        private AsyncOperationHandle<GameObject> _levelButtonOperationHandle = new();

        public LevelButtonFactory(AssetReference levelButtonPrefab)
        {
            _levelButtonPrefab = levelButtonPrefab;
        }

        public async UniTask<LevelButtonView> CreateAsynс(Transform parent, CancellationToken token)
        {
            _levelButtonOperationHandle = Addressables.InstantiateAsync(_levelButtonPrefab, parent);
            var levelInstance = await _levelButtonOperationHandle.WithCancellation(token);

            return levelInstance.GetComponent<LevelButtonView>();
        }

        public void Dispose()
        {
            Addressables.Release(_levelButtonOperationHandle);
        }
    }
}