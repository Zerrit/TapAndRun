using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TapAndRun.Configs
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "Levels/Levels Config")]
    public class LevelsConfig : ScriptableObject
    {
        [field:SerializeField] public AssetReference[] LevelPrefabs { get; private set; }
    }
}