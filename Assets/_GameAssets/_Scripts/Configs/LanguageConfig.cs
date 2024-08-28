using UnityEngine;

namespace TapAndRun.Configs
{
    [CreateAssetMenu(fileName = "LangConfig", menuName = "Language/Lang Config")]
    public class LanguageConfig : ScriptableObject
    {
        [field:SerializeField] public string Id { get; private set; }
        [field:SerializeField] public Sprite Icon { get; private set; }
    }
}