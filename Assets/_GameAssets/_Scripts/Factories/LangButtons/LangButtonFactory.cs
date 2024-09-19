using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using TapAndRun.Interfaces;
using TapAndRun.MVP.Settings.Views;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TapAndRun.Factories.LangButtons
{
    public class LangButtonFactory: ILangButtonFactory, IDecomposable
    {
        private AsyncOperationHandle<GameObject> _langButtonPrefabOperationHandle;

        private readonly AssetReference _langButtonRef;
        private readonly LanguagePoolConfig _langPoolConfig;

        public LangButtonFactory(AssetReference langButtonRef, LanguagePoolConfig langPoolConfig)
        {
            _langButtonRef = langButtonRef;
            _langPoolConfig = langPoolConfig;
        }

        public int GetLangCount()
        {
            var count = _langPoolConfig._langPool.Length;

            if (count <= 0)
            {
                throw new Exception("Не найдено языковых конфигов!");
            }

            return count;
        }

        public async UniTask<LangButton> CreateAsynс(int index, Transform parent, CancellationToken token)
        {
            if (!_langButtonPrefabOperationHandle.IsValid())
            {
                _langButtonPrefabOperationHandle = Addressables.LoadAssetAsync<GameObject>(_langButtonRef);

                await _langButtonPrefabOperationHandle.WithCancellation(token);
            }

            var instance = await Addressables.InstantiateAsync(_langButtonRef, parent).WithCancellation(token);

            var langButton =  instance.GetComponent<LangButton>();
            langButton.Initialize(_langPoolConfig._langPool[index]);

            return langButton;
        }

        public void Decompose()
        {
            if (_langButtonPrefabOperationHandle.IsValid())
            {
                Addressables.Release(_langButtonPrefabOperationHandle);
            }
        }
    }
}