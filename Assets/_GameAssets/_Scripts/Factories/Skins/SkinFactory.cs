using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TapAndRun.Factories.Skins
{
    public class SkinFactory : ISkinFactory, IDisposable
    {
        public SkinsConfig SkinsConfig { get; }

        private AsyncOperationHandle<GameObject> _usingSkinOperationHandle;

        private readonly List<AsyncOperationHandle<GameObject>> _assortmentOperationHandles = new();

        public SkinFactory(SkinsConfig skinsConfig)
        {
            SkinsConfig = skinsConfig;
        }

        public async UniTask<List<GameObject>> CreateAllSkinsAsync(Transform parent, CancellationToken token)
        {
            var skins = new List<GameObject>();

            foreach (var skinData in SkinsConfig.SkinsData)
            {
                var skinsOperationHandle = Addressables.InstantiateAsync(skinData.SkinPrefabRef, parent);
                var instance = await skinsOperationHandle.WithCancellation(token);

                if (skinsOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    _assortmentOperationHandles.Add(skinsOperationHandle);
                    skins.Add(instance);
                }
                else throw new Exception($"Skin asset loading was finished with {skinsOperationHandle.Status}");
            }

            return skins;
        }
        
        public async UniTask<GameObject> ChangeSkinTo(string name, Transform parent, CancellationToken token)
        {
            foreach (var skinData in SkinsConfig.SkinsData)
            {
                if (skinData.Name.Equals(name))
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

        public void DisposeUnusedSkins()
        {
            foreach (var skin in _assortmentOperationHandles)
            {
                Addressables.Release(skin);
            }

            _assortmentOperationHandles.Clear();
        }
        
        public void Dispose()
        {
            Addressables.Release(_usingSkinOperationHandle);
            DisposeUnusedSkins();
        }
    }
}