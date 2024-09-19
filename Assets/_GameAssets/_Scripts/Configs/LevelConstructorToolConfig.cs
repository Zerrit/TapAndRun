using TapAndRun.MVP.Levels.Views.SegmentViews;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TapAndRun.Configs
{
    [CreateAssetMenu(fileName = "LevelConstructorToolConfig", menuName = "Tools/Level Constructor Tool Config")]
    public class LevelConstructorToolConfig : ScriptableObject
    {
        [field:SerializeField] public StartSegmentView StartSegment { get; private set; }
        [field:SerializeField] public RoadSegmentView ShortRoadSegment { get; private set; }
        [field:SerializeField] public RoadSegmentView MiddleRoadSegment { get; private set; }
        [field:SerializeField] public RoadSegmentView LongRoadSegment { get; private set; }
        [field:SerializeField] public InteractSegmentView LeftTurnSegment { get; private set; }
        [field:SerializeField] public InteractSegmentView RightTurnSegment { get; private set; }
        [field:SerializeField] public InteractSegmentView JumpStartSegment { get; private set; }
        [field:SerializeField] public InteractSegmentView JumpSegment { get; private set; }
        [field:SerializeField] public RoadSegmentView JumpEndSegment { get; private set; }
        [field:SerializeField] public FinishSegmentView FinishSegment { get; private set; }
    }
}