using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Interfaces;
using TapAndRun.MVP.MainMenu.Views;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TapAndRun.Factories.LevelButtons
{
    public class LevelButtonFactory : ILevelButtonFactory, IDecomposable
    {
        private readonly AssetReference _levelButtonRef;

        private AsyncOperationHandle<GameObject> _levelButtonPrefabOperationHandle;

        public LevelButtonFactory(AssetReference levelButtonPrefab)
        {
            _levelButtonRef = levelButtonPrefab;
        }

        public async UniTask<LevelButtonView> CreateAsynс(Transform parent, CancellationToken token)
        {
            if (!_levelButtonPrefabOperationHandle.IsValid())
            {
                _levelButtonPrefabOperationHandle = Addressables.LoadAssetAsync<GameObject>(_levelButtonRef);

                await _levelButtonPrefabOperationHandle.WithCancellation(token);
            }

            var levelInstance = await Addressables.InstantiateAsync(_levelButtonRef, parent).WithCancellation(token);

            return levelInstance.GetComponent<LevelButtonView>();
        }

        public void Decompose()
        {
            if (_levelButtonPrefabOperationHandle.IsValid())
            {
                Addressables.Release(_levelButtonPrefabOperationHandle);
            }
        }
    }
}