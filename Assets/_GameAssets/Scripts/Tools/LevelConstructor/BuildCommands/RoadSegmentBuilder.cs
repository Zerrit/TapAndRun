using TapAndRun.Configs;
using TapAndRun.MVP.Levels.View;
using UnityEngine;

namespace TapAndRun.Tools.LevelConstructor.BuildCommands
{
    public class RoadSegmentBuilder : AbstractSegmentBuilder
    {
        private int _roadSize;
        private readonly AbstractSegmentView _segmentPrefab;

        public RoadSegmentBuilder(LevelConstructorToolConfig config, LevelView levelView, int size)
        {
            _segmentPrefab = config.RoadSegment;
            _level = levelView;
            _roadSize = size;
        }

        public override void Build()
        {
            var snapPoint = _level.FinishSegment.SnapPoint;

            _instance = Object.Instantiate(_segmentPrefab, snapPoint.position, snapPoint.rotation, _level.transform);
            _instance.transform.Rotate(Vector3.forward, _level.FinishSegment.SnapAngleOffset);

            _level.Segments.Add(_instance);
            _level.FinishSegment = _instance;
        }
    }
}