using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TapAndRun.Configs;
using UnityEngine;

namespace TapAndRun.Factories.Skins
{
    public interface ISkinFactory
    {
        SkinsConfig SkinsConfig { get; }

        UniTask<List<GameObject>> CreateAllSkinsAsync(Transform parent, CancellationToken token);
        UniTask<GameObject> ChangeSkinTo(string name, Transform parent, CancellationToken token);
        void ReleaseUnusedSkins();
    }
}