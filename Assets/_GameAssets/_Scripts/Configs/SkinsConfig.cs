using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TapAndRun.Configs
{
    [CreateAssetMenu(fileName = "SkinsConfig", menuName = "Character/Skins Config")]
    public class SkinsConfig : ScriptableObject
    {
        [field: SerializeField] public SkinData[] SkinsData { get; private set; }
    }
    
    [Serializable]
    public class SkinData
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public AssetReference SkinPrefabRef { get; private set; }
    }
}