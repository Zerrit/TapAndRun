using UnityEngine;

namespace TapAndRun.MVP.Levels.View
{
    public abstract class AbstractSegmentView : MonoBehaviour
    {
        public LastSegmentType SegmentType { get; private set; }

        public abstract void Reset();
    }
}