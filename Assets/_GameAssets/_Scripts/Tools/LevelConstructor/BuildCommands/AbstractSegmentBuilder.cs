using TapAndRun.MVP.Levels.View;
using UnityEngine;

namespace TapAndRun.Tools.LevelConstructor.BuildCommands
{
    public abstract class AbstractSegmentBuilder
    {
        protected AbstractSegmentView _instance;
        protected AbstractSegmentView _segmentPrefab;

        protected readonly LevelView _level;

        protected AbstractSegmentBuilder(LevelView level)
        {
            _level = level;
        }

        public virtual void Build()
        {
            var segmentsCount = _level.Segments.Count;

            if (segmentsCount > 0)
            {
                var snapPoint = _level.Segments[segmentsCount-1].SnapPoint;

                _instance = Object.Instantiate(_segmentPrefab, snapPoint.position, snapPoint.rotation, _level.transform);
                _instance.transform.Rotate(Vector3.forward, _level.Segments[segmentsCount-1].SnapAngleOffset);
            }
            else
            {
                var snapPoint = _level.StartSegment.SnapPoint;

                _instance = Object.Instantiate(_segmentPrefab, snapPoint.position, snapPoint.rotation, _level.transform);
            }

            _level.Segments.Add(_instance);
        }

        public virtual void Debuild()
        {
            _level.Segments.Remove(_instance);

            if (_instance)
            {
                Object.DestroyImmediate(_instance.gameObject);
            }
        }
    }
}