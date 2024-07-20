using UnityEngine;

namespace TapAndRun.MVP.Levels.View
{
    public class RoadSegmentView : AbstractSegmentView
    {

        [field: SerializeField] public CrystalView[] Crystals { get; private set; }
        public override void ResetSegment()
        {
            
        }
    }
}