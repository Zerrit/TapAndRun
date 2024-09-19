using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TapAndRun.MVP.Skins_Shop.Model
{
    [Serializable]
    public class SkinData
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public AssetReference SkinPrefabRef { get; private set; }
    }
}