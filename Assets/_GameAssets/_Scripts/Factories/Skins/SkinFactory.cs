using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using TapAndRun.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TapAndRun.Factories.Skins
{
    public class SkinFactory : ISkinFactory, IDecomposable
    {
        public SkinsConfig SkinsConfig { get; }

        private AsyncOperationHandle<GameObject> _usingSkinOperationHandle;
        private AsyncOperationHandle<GameObject> _skinHolderOperationHandle;

        private readonly List<AsyncOperationHandle<GameObject>> _assortmentOperationHandles = new();

        public SkinFactory(SkinsConfig skinsConfig)
        {
            SkinsConfig = skinsConfig;
        }

        public async UniTask<List<GameObject>> CreateAllSkinsAsync(Transform parent, CancellationToken token)
        {
            var skinsHolders = new List<GameObject>();

            _skinHolderOperationHandle = Addressables.LoadAssetAsync<GameObject>(SkinsConfig.SkinHolderRef);
            await _skinHolderOperationHandle.WithCancellation(token);

            foreach (var skinData in SkinsConfig.SkinsData)
            {
                var skinHolderInstance = await Addressables.InstantiateAsync(SkinsConfig.SkinHolderRef, parent).WithCancellation(token);

                var skinsOperationHandle = Addressables.InstantiateAsync(skinData.SkinPrefabRef, skinHolderInstance.transform);
                await skinsOperationHandle.WithCancellation(token);

                if (skinsOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    _assortmentOperationHandles.Add(skinsOperationHandle);
                    skinsHolders.Add(skinHolderInstance);
                }
                else throw new Exception($"Skin asset loading was finished with {skinsOperationHandle.Status}");
            }

            return skinsHolders;
        }

        public async UniTask<GameObject> ChangeSkinTo(string id, Transform parent, CancellationToken token)
        {
            foreach (var skinData in SkinsConfig.SkinsData)
            {
                if (skinData.Id.Equals(id))
                {
                    var skinsOperationHandle = Addressables.InstantiateAsync(skinData.SkinPrefabRef, parent);
                    var instance = await skinsOperationHandle.WithCancellation(token);

                    if (skinsOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (_usingSkinOperationHandle.IsValid())
                        {
                            Addressables.Release(_usingSkinOperationHandle);
                        }

                        _usingSkinOperationHandle = skinsOperationHandle;

                        return instance;
                    }

                    throw new Exception($"Skin asset loading was finished with {skinsOperationHandle.Status}");
                }
            }

            throw new Exception("No skins found in the config ");
        }

        public void ReleaseSkinShopAssortment()
        {
            foreach (var skin in _assortmentOperationHandles)
            {
                Addressables.Release(skin);
            }

            _assortmentOperationHandles.Clear();
        }

        public void Decompose()
        {
            Addressables.Release(_usingSkinOperationHandle);
            ReleaseSkinShopAssortment();
        }
    }
}