using TapAndRun.MVP.Levels.View;
using TapAndRun.MVP.Levels.View.SegmentViews;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TapAndRun.Configs
{
    [CreateAssetMenu(fileName = "LevelConstructorToolConfig", menuName = "Tools/Level Constructor Tool Config")]
    public class LevelConstructorToolConfig : ScriptableObject
    {
        [field:SerializeField] public StartSegmentView StartSegment { get; private set; }
        [field:SerializeField] public AbstractSegmentView RoadSegment { get; private set; }
        [field:SerializeField] public AbstractSegmentView LeftTurnSegment { get; private set; }
        [field:SerializeField] public AbstractSegmentView RightTurnSegment { get; private set; }
        [field:SerializeField] public AbstractSegmentView JumpStartSegment { get; private set; }
        [field:SerializeField] public AbstractSegmentView JumpSegment { get; private set; }
        [field:SerializeField] public AbstractSegmentView JumpEndSegment { get; private set; }
        [field:SerializeField] public FinishSegmentView FinishSegment { get; private set; }
    }
}