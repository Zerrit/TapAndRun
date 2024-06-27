using UnityEngine;

namespace TapAndRun.Levels.View
{
    public class SegmentView : MonoBehaviour
    {
        [SerializeField] private Transform _selfTtransform;
        public LastSegmentType type;

        public SegmentView(Transform segment, LastSegmentType type)
        {
            this._selfTtransform = segment;
            this.type = type;
        }
    }
}