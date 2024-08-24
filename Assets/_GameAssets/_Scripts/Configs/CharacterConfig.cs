using UnityEngine;

namespace TapAndRun.Configs
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Character/Character Config")]
    public class CharacterConfig : ScriptableObject
    {
        [field: SerializeField] public float BaseMoveSpeed { get; private set; }
        [field: SerializeField] public float TurnSpeed { get; private set; }
        [field: SerializeField] public float CenteringSpeed { get; private set; }
        [field: SerializeField] public Vector3 RoadCheckerOffset { get; private set; }
    }
}