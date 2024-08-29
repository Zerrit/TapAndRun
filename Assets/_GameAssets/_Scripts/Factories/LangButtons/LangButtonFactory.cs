using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using TapAndRun.MVP.MainMenu.Views;
using TapAndRun.MVP.Settings.Views;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TapAndRun.Factories.LangButtons
{
    public class LangButtonFactory: ILangButtonFactory, IDisposable
    {
        private readonly AssetReference _langButtonPrefab;
        private readonly LanguagePoolConfig _langPoolConfig;

        private AsyncOperationHandle<GameObject> _langButtonOperationHandle = new();

        public LangButtonFactory(AssetReference langButtonPrefab, LanguagePoolConfig langPoolConfig)
        {
            _langButtonPrefab = langButtonPrefab;
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
            _langButtonOperationHandle = Addressables.InstantiateAsync(_langButtonPrefab, parent);
            var levelInstance = await _langButtonOperationHandle.WithCancellation(token);

            var langButton =  levelInstance.GetComponent<LangButton>();
            langButton.Initialize(_langPoolConfig._langPool[index]);

            return langButton;
        }

        public void Dispose()
        {
            Addressables.Release(_langButtonOperationHandle);
        }
    }
}