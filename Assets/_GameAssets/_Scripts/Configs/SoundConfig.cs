using TapAndRun.Audio;
using UnityEngine;

namespace TapAndRun.Configs
{
    [CreateAssetMenu(fileName = "SoundConfig", menuName = "Audio/Sound Config")]
    public class SoundConfig : ScriptableObject
    {
        [field:SerializeField] public Sound[] Sounds { get; private set; }
    }
}