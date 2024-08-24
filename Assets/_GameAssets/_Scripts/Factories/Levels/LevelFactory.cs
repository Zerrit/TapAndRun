using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using TapAndRun.MVP.Levels.Views;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TapAndRun.Factories.Levels
{
    public class LevelFactory : ILevelFactory, IDisposable
    {
        private readonly LevelsConfig _levelsConfig;
        private readonly Transform _parentContainer;

        private readonly Queue<AsyncOperationHandle<GameObject>> _levelOperationHandles = new();

        public LevelFactory(LevelsConfig levelsConfig, Transform parentContainer)
        {
            _levelsConfig = levelsConfig;
            _parentContainer = parentContainer;
        }

        public int GetLevelCount()
        {
            return _levelsConfig.LevelPrefabs.Length;
        }

        public async UniTask<LevelView> CreateLevelViewAsync(int levelId, Vector2 position, Quaternion rotation, CancellationToken token)
        {
            var assetRef = _levelsConfig.LevelPrefabs[levelId];

            var operationHandle = Addressables.InstantiateAsync(assetRef, position, rotation, _parentContainer);
            var levelInstance = await operationHandle.WithCancellation(token);

            _levelOperationHandles.Enqueue(operationHandle);

            return levelInstance.GetComponent<LevelView>();
        }

        public void Dispose()
        {
            foreach (var handle in _levelOperationHandles)
            {
                Addressables.Release(handle);
            }

            _levelOperationHandles.Clear();
        }
    }
}