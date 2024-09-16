using TapAndRun.MVP.Skins_Shop.Model;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TapAndRun.Configs
{
    [CreateAssetMenu(fileName = "SkinsConfig", menuName = "Character/Skins Config")]
    public class SkinsConfig : ScriptableObject
    {
        [field: SerializeField] public AssetReference SkinHolderRef { get; private set; }

        [field: SerializeField] public SkinData[] SkinsData { get; private set; }
    }
}