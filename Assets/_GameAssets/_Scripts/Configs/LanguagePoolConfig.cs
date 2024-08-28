using UnityEngine;

namespace TapAndRun.Configs
{
    [CreateAssetMenu(fileName = "LanguagePoolConfig", menuName = "Language/Lang Pool Config")]
    public class LanguagePoolConfig : ScriptableObject
    {
        [field: SerializeField] public LanguageConfig[] _langPool;
    }
}