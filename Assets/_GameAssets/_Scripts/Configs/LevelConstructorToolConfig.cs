using TapAndRun.MVP.Gameplay.Views.SegmentViews;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TapAndRun.Configs
{
    [CreateAssetMenu(fileName = "LevelConstructorToolConfig", menuName = "Tools/Level Constructor Tool Config")]
    public class LevelConstructorToolConfig : ScriptableObject
    {
        [field:SerializeField] public StartSegmentView StartSegment { get; private set; }
        [field:SerializeField] public RoadSegmentView RoadSegment { get; private set; }
        [field:SerializeField] public InteractSegmentView LeftTurnSegment { get; private set; }
        [field:SerializeField] public InteractSegmentView RightTurnSegment { get; private set; }
        [field:SerializeField] public InteractSegmentView JumpStartSegment { get; private set; }
        [field:SerializeField] public InteractSegmentView JumpSegment { get; private set; }
        [field:SerializeField] public RoadSegmentView JumpEndSegment { get; private set; }
        [field:SerializeField] public FinishSegmentView FinishSegment { get; private set; }
    }
}