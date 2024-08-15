using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.MVP.Levels.View;
using TapAndRun.MVP.Screens.LevelSelect;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TapAndRun.Factories.LevelButtons
{
    public interface ILevelButtonFactory
    {
    }

    public class LevelButtonFactory : ILevelButtonFactory
    {
        private readonly LevelButtonView _levelButtonPrefab;

        private readonly AsyncOperationHandle<GameObject> _levelButtonOperationHandle = new();

        public LevelButtonFactory(LevelButtonView levelButtonPrefab)
        {
            _levelButtonPrefab = levelButtonPrefab;
        }

        public async UniTask<LevelButtonView> CreateLevelViewAsync(Vector2 position, Quaternion rotation, Transform parent, CancellationToken token)
        {
            var operationHandle = Addressables.InstantiateAsync(_levelButtonPrefab, position, rotation, parent);
            var levelInstance = await operationHandle.WithCancellation(token);

            return levelInstance.GetComponent<LevelButtonView>();
        }

        public void Dispose()
        {
            Addressables.Release(_levelButtonOperationHandle);
        }
    }
}